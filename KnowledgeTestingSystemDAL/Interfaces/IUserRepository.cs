using KnowledgeTestingSystemDAL.Entities;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
