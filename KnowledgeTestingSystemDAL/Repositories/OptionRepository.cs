using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class OptionRepository : IOptionRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public OptionRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(Option entity)
        {
            await _knowledgeTestingSystemDbContext.Options.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Options.FindAsync(id);
            element.IsDeleted = true;
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();  
        }
        public async Task<IEnumerable<Option>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.Options.Where(option => !option.IsDeleted).ToListAsync();
        }

        public async Task<Option> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Options.FindAsync(id);
            return element;
        }
        public async Task<IEnumerable<Option>> GetByQuestionIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Options.Where(x => x.QuestionId == id && !x.IsDeleted).ToListAsync();
            return element;
        }

        public async Task<Option> UpdateAsync(Option entity)
        {
            var element = await _knowledgeTestingSystemDbContext.Options.FirstOrDefaultAsync(x => x.Id == entity.Id);
            element.Text = entity.Text;
            element.IsCorrect = entity.IsCorrect;
            element.QuestionId = entity.QuestionId;

            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
