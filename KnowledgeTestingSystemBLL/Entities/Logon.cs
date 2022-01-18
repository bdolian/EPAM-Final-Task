using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class Logon
    {
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }
    }
}
