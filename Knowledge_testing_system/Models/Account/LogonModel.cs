using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystem.Models.Account
{
    public class LogonModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
