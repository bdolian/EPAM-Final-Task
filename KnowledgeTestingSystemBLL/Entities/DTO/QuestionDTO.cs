using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter question text")]
        public string Text { get; set; }
        public int TestId { get; set; }
        public int NumberOfOptions { get; set; } = 0;
        [Required]
        [MinLength(2,ErrorMessage = "Enter at least 2 options")]
        public OptionDTO[] Options { get; set; } = null;
    }
}
