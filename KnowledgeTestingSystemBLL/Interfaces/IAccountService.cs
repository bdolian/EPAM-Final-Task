using KnowledgeTestingSystemBLL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface IAccountService
    {
        Task Register(Register user);
        Task<ApplicationUser> Logon(Logon logonUser);
        Task<IdentityResult> DeleteUser(ApplicationUser user);
    }
}
