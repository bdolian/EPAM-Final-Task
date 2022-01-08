﻿using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class TestRepository : ITestRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public TestRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(Test entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            await _knowledgeTestingSystemDbContext.Tests.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Tests.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            if (element.IsDeleted)
                throw new ArgumentException("This test is already deleted(how did you get there?)");

            element.IsDeleted = true;
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Test>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.Tests.Where(test => !test.IsDeleted).ToListAsync();
        }

        public async Task<Test> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Tests.FindAsync(id);

            if (element == null || element.IsDeleted)
                throw new ArgumentException("There is no such test");

            return element;
        }

        public async Task<Test> UpdateAsync(Test entity)
        {
            var element = await _knowledgeTestingSystemDbContext.Tests.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (element == null || element.IsDeleted)
                throw new ArgumentException("There is no such test");

            element.TimeToEnd = entity.TimeToEnd;
            element.NumberOfQuestions = entity.NumberOfQuestions;
            element.Name = entity.Name;

            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
