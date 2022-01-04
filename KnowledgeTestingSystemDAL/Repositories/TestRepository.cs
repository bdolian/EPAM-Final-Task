using KnowledgeTestingSystemDAL.Entities;
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

            _knowledgeTestingSystemDbContext.Tests.Remove(element);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<Test>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.Tests.ToListAsync();
        }

        public async Task<Test> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Tests.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            return element;
        }

        public async Task<Test> UpdateAsync(Test entity)
        {
            var element = await _knowledgeTestingSystemDbContext.Tests.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
