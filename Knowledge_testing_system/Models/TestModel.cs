using System;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystem.Models
{
    public class TestModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the name of the test")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter number of questions")]
        [Range(1, 15)]
        public int NumberOfQuestions { get; set; }

        [Required(ErrorMessage = "Please enter time required to pass the test")]
        public DateTime TimeToEnd { get; set; }
    }
}
