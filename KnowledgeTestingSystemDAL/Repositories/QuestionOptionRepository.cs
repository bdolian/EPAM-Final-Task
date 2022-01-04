using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class QuestionOptionRepository : IQuestionOptionRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public QuestionOptionRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(QuestionOption entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            await _knowledgeTestingSystemDbContext.QuestionsOptions.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.QuestionsOptions.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            _knowledgeTestingSystemDbContext.QuestionsOptions.Remove(element);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<QuestionOption>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.QuestionsOptions.ToListAsync();
        }

        public async Task<QuestionOption> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.QuestionsOptions.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            return element;
        }

        public async Task<QuestionOption> UpdateAsync(QuestionOption entity)
        {
            var element = await _knowledgeTestingSystemDbContext.QuestionsOptions.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
