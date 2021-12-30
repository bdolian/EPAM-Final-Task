using KnowledgeTestingSystemBLL.Entities;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface IUserService
    {
        Task Register(Register user);
        Task<ApplicationUser> Logon(Logon logonUser);
    }
}
