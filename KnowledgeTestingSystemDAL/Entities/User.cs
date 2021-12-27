using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(15)]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set;}
        [Required]
        public string Email { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
