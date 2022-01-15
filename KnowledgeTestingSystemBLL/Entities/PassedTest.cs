namespace KnowledgeTestingSystemBLL.Entities
{
    public class PassedTest
    {
        public int TestId { get; set; }
        public QuestionAnswer[] QuestionAnswers { get; set; }
    }
}
