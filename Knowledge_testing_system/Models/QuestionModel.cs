using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystem.Models
{
    public class QuestionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the question text")]
        [MaxLength(100)]
        public string Text { get; set; }
        public int TestId { get; set; }
        public int NumberOfOptions { get; set; }

        [Required(ErrorMessage = "Add some options!")]
        [MinLength(2)]
        public OptionModel[] Options { get; set; }
    }
}
