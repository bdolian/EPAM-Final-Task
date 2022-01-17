using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class PassedTest
    {
        [Required]
        public int TestId { get; set; }
        [Required]
        [MinLength(1)]
        public QuestionAnswer[] QuestionAnswers { get; set; }
    }
}
