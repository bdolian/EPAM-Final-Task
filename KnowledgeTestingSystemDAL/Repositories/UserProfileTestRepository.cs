using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class UserProfileTestRepository : IUserProfileTestRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public UserProfileTestRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(UserProfileTest entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            await _knowledgeTestingSystemDbContext.UserProfilesTests.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfilesTests.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            _knowledgeTestingSystemDbContext.UserProfilesTests.Remove(element);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public IEnumerable<UserProfileTest> GetAll()
        {
            return _knowledgeTestingSystemDbContext.UserProfilesTests.ToList();
        }

        public async Task<UserProfileTest> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfilesTests.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            return element;
        }

        public async Task<UserProfileTest> UpdateAsync(UserProfileTest entity)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfilesTests.FirstOrDefaultAsync(x => x.Id == entity.Id);
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
