using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "Please enter your first name")]
        [MaxLength(15)]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter your second name")]
        public string LastName { get; set;}
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
