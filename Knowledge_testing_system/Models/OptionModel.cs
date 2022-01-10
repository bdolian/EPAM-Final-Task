using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystem.Models
{
    public class OptionModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the option text")]
        public string Text { get; set; }
        public string IsCorrect { get; set; } = "false";
        public int QuestionId { get; set; }
    }
}
