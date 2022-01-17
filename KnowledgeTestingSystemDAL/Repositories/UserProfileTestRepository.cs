using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
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
            await _knowledgeTestingSystemDbContext.UserProfilesTests.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfilesTests.FindAsync(id);

            element.IsDeleted = true;
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }

        public async Task DeleteByUserProfileIdAsync(int id)
        {
            var elements = await _knowledgeTestingSystemDbContext.UserProfilesTests.Where(x => x.UserProfileId == id).ToListAsync();

            foreach (var item in elements)
            {
                item.IsDeleted = true;
                _knowledgeTestingSystemDbContext.Entry(item).State = EntityState.Modified;
            }
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserProfileTest>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.UserProfilesTests
                .Where(userProfileTest => !userProfileTest.IsDeleted).ToListAsync();
        }

        public async Task<UserProfileTest> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfilesTests.FindAsync(id);
            return element;
        }

        public async Task<IEnumerable<UserProfileTest>> GetByUserIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfiles.Where(x => x.UserId == id).FirstOrDefaultAsync();

            var userProfileTest = await _knowledgeTestingSystemDbContext.UserProfilesTests.Where(x => x.UserProfileId == element.Id)
                                                                                          .ToListAsync();

            return userProfileTest;
        }

        public async Task<UserProfileTest> UpdateAsync(UserProfileTest entity)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfilesTests.FirstOrDefaultAsync(x => x.Id == entity.Id);

            element.NumberOfAttempts = entity.NumberOfAttempts;
            element.IsDeleted = entity.IsDeleted;
            element.UserProfileId = entity.UserProfileId;
            element.Grade = entity.Grade;
            element.TestId = entity.TestId;

            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
