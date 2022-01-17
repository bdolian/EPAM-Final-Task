using KnowledgeTestingSystemDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IUserProfileTestRepository : IRepository<UserProfileTest>
    {
        Task<IEnumerable<UserProfileTest>> GetByUserIdAsync(int id);
        Task DeleteByUserProfileIdAsync(int id);
    }
}
