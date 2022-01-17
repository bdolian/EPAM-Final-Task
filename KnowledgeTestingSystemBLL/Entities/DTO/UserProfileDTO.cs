using System;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities.DTO
{
    public class UserProfileDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter date of birth")]
        public DateTime DateOfBirth { get; set; }
        public int UserId { get; set; }
    }
}
