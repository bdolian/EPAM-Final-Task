using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL
{
    public class AssignUserToRoles
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [MinLength(1)]
        public string[] Roles { get; set; }
    }
}
