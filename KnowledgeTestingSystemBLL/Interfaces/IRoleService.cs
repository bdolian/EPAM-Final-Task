using KnowledgeTestingSystemBLL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface IRoleService
    {
        Task AssignUserToRoles(AssignUserToRoles assignUserToRoles);
        Task CreateRole(string roleName);
        Task<IEnumerable<string>> GetRoles(ApplicationUser user);
        Task<IEnumerable<IdentityRole>> GetRoles();
    }
}
