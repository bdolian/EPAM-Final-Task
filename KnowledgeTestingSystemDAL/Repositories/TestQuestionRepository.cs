using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class TestQuestionRepository : ITestQuestionRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public TestQuestionRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(TestQuestion entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            await _knowledgeTestingSystemDbContext.TestsQuestions.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.TestsQuestions.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            _knowledgeTestingSystemDbContext.TestsQuestions.Remove(element);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<TestQuestion>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.TestsQuestions.ToListAsync();
        }

        public async Task<TestQuestion> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.TestsQuestions.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            return element;
        }

        public async Task<TestQuestion> UpdateAsync(TestQuestion entity)
        {
            var element = await _knowledgeTestingSystemDbContext.TestsQuestions.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
