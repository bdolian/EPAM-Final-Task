using KnowledgeTestingSystemBLL.Entities.DTO;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class TestDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfQuestions { get; set; }
        public int TimeToEnd { get; set; }
        public QuestionDTO[] Questions { get; set; } = null;
    }
}
