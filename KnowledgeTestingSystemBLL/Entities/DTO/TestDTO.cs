using KnowledgeTestingSystemBLL.Entities.DTO;
using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class TestDTO
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the name of the test")]
        public string Name { get; set; }
        public int NumberOfQuestions { get; set; }
        [Required(ErrorMessage = "Please enter average time to end the test(in minutes)")]
        public string TimeToEnd { get; set; }
        [Required(ErrorMessage = "Please add at least 1 question")]
        [MinLength(1, ErrorMessage = "Please add at least 1 question")]
        public QuestionDTO[] Questions { get; set; } = null;
    }
}
