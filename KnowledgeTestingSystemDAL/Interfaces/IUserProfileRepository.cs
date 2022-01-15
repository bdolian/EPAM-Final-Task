using KnowledgeTestingSystemDAL.Entities;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IUserProfileRepository : IRepository<UserProfile>
    {
        Task<UserProfile> GetByUserIdAsync(int id);
        Task DeleteByUserIdAsync(int id);
    }
}
