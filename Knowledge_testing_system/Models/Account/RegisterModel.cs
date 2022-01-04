using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystem.Models.Account
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [MaxLength(15)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your second name")]
        public string LastName { get; set; }

    }
}
