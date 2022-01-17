using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class UserDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please enter last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        public string Email { get; set; }
    }
}
