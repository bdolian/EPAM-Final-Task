using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KnowledgeTestingSystemDbContext _knowledgeTestingSystemDbContext;

        public UserRepository(KnowledgeTestingSystemDbContext knowledgeTestingSystemDbContext)
        {
            _knowledgeTestingSystemDbContext = knowledgeTestingSystemDbContext;
        }
        public async Task AddAsync(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException();

            await _knowledgeTestingSystemDbContext.Users.AddAsync(entity);
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Users.FindAsync(id);

            if (element == null)
                throw new ArgumentNullException();

            if (element.IsDeleted)
                throw new ArgumentException("This user is already deleted");

            element.IsDeleted = true;
            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _knowledgeTestingSystemDbContext.Users
                .Where(user => !user.IsDeleted).ToListAsync();
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var element = await _knowledgeTestingSystemDbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (element == null)
                throw new ArgumentNullException();

            return element;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var element = await _knowledgeTestingSystemDbContext.Users.FindAsync(id);

            if (element == null || element.IsDeleted)
                throw new ArgumentNullException("There is no such user");

            return element;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            var element = await _knowledgeTestingSystemDbContext.Users.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (element == null || element.IsDeleted)
                throw new ArgumentException("There is no such user");

            element.FirstName = entity.FirstName;
            element.LastName = entity.LastName;
            element.Email = entity.Email;

            _knowledgeTestingSystemDbContext.Entry(element).State = EntityState.Modified;
            await _knowledgeTestingSystemDbContext.SaveChangesAsync();
            return element;
        }
    }
}
