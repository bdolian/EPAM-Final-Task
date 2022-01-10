namespace KnowledgeTestingSystemBLL.Entities.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int TestId { get; set; }
        public int NumberOfOptions { get; set; } = 0;
        public OptionDTO[] Options { get; set; } = null;
    }
}
