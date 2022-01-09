using KnowledgeTestingSystemBLL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface IUserService : IService<UserDTO>
    {
        Task<UserCompleteInformation> GetWithProfileAsync(int id);
    }
}
