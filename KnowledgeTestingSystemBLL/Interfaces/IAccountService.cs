using KnowledgeTestingSystemBLL.Entities;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface IAccountService
    {
        Task Register(Register user);
        Task<ApplicationUser> Logon(Logon logonUser);
    }
}
