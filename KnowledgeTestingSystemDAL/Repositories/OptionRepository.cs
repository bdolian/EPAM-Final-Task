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
            if (entity == null)
                throw new ArgumentNullException();

            await _knowledgeTestingSystemDbContext.Options.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Options.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            if (element.IsDeleted)
                throw new ArgumentException("This option is already deleted");

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

            if (element == null || element.IsDeleted)
                throw new ArgumentNullException("There is no such option");

            return element;
        }
        public async Task<IEnumerable<Option>> GetByQuestionIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Options.Where(x => x.QuestionId == id && !x.IsDeleted).ToListAsync();

            if (element == null)
                throw new ArgumentNullException("There is no such option");

            return element;
        }

        public async Task<Option> UpdateAsync(Option entity)
        {
            var element = await _knowledgeTestingSystemDbContext.Options.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (element == null || element.IsDeleted)
                throw new ArgumentNullException("There is no such option");

            element.Text = entity.Text;
            element.QuestionId = entity.QuestionId;

            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
