using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public UserProfileRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(UserProfile entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            await _knowledgeTestingSystemDbContext.UserProfiles.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfiles.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            if (element.IsDeleted)
                throw new ArgumentException("This user profile is already deleted");

            element.IsDeleted = true;
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();     
        }

        public async Task DeleteByUserIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfiles.Where(x => x.UserId == id).FirstOrDefaultAsync();

            if (element == null)
                throw new ArgumentNullException();

            if (element.IsDeleted)
                throw new ArgumentException("This user profile is already deleted");

            element.IsDeleted = true;
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.UserProfiles.Where(userProfile => !userProfile.IsDeleted).ToListAsync();
        }

        public async Task<UserProfile> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfiles.FindAsync(id);

            if (element == null || element.IsDeleted)
                throw new ArgumentNullException("There is no such user profile");

            return element;
        }

        public async Task<UserProfile> GetByUserIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfiles.Where(x => x.UserId == id).FirstOrDefaultAsync();

            if (element == null || element.IsDeleted)
                throw new ArgumentNullException("There is no such profile");

            return element;
        }

        public async Task<UserProfile> UpdateAsync(UserProfile entity)
        {
            var element = await _knowledgeTestingSystemDbContext.UserProfiles.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (element == null || element.IsDeleted)
                throw new ArgumentException("There is no such user profile");

            element.DateOfBirth = entity.DateOfBirth;
            element.UserId = entity.UserId;

            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
