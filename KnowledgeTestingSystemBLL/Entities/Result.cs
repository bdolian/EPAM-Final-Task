namespace KnowledgeTestingSystemBLL.Entities
{
    public class Result
    {
        public int TestId { get; set; }
        public int Grade { get; set; }
        public QuestionAnswer[] QuestionAnswers { get; set; }
    }
}
