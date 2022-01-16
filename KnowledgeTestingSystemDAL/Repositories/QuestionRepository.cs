using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public QuestionRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(Question entity)
        {
            await _knowledgeTestingSystemDbContext.Questions.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Questions.FindAsync(id);

            element.IsDeleted = true;
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();  
        }
        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.Questions.Where(question => !question.IsDeleted).ToListAsync();
        }

        public async Task<Question> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Questions.FindAsync(id);

            return element;
        }

        public async Task<IEnumerable<Question>> GetByTestIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Questions.Where(x => x.TestId == id && !x.IsDeleted).ToListAsync();

            return element;
        }

        public async Task<Question> UpdateAsync(Question entity)
        {
            var element = await _knowledgeTestingSystemDbContext.Questions.FirstOrDefaultAsync(x => x.Id == entity.Id);

            element.Text = entity.Text;
            element.TestId = entity.TestId;
            element.NumberOfOptions = entity.NumberOfOptions;

            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
