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
        public int NumberOfQuestions { get; set; }

        [Required(ErrorMessage = "Please enter time required to pass the test")]
        public string TimeToEnd { get; set; }

        [Required(ErrorMessage = "Please add at least 1 question")]
        public QuestionModel[] Questions { get; set; }
    }
}
