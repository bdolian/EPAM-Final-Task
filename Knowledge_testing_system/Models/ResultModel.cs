namespace KnowledgeTestingSystem.Models
{
    public class ResultModel
    {
        public int TestId { get; set; }
        public int Grade { get; set; }
        public QuestionAnswerModel[] QuestionAnswers { get; set; }
    }
}
