using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class QuestionAnswer
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int AnswerId { get; set; }
    }
}
