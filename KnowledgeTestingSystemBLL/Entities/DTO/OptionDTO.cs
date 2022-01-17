using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities.DTO
{
    public class OptionDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter option text")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Please enter if option is correct")]
        public string IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}
