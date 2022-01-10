namespace KnowledgeTestingSystemBLL.Entities.DTO
{
    public class OptionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string IsCorrect { get; set; }
        public int QuestionId { get; set; }
    }
}
