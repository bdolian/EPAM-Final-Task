using KnowledgeTestingSystemBLL.Entities;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface IUserService : IService<UserDTO>
    {
        Task<UserCompleteInformation> GetWithProfileAsync(int id);
        Task<UserCompleteInformation> EditCompleteAsync(UserCompleteInformation entity);
        Task AddUserProfileTest(Result result, string userEmail);
    }
}
