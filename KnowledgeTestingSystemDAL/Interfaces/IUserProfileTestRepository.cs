using KnowledgeTestingSystemDAL.Entities;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IUserProfileTestRepository : IRepository<UserProfileTest>
    {
        Task<UserProfileTest> GetByUserIdAsync(int id);
        Task DeleteByUserProfileIdAsync(int id);
    }
}
