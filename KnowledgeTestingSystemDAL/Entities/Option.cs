namespace KnowledgeTestingSystemDAL.Entities
{
    public class Option : BaseEntity
    {
        public string Text { get; set; }
        public bool IsCorrect { get; set; } = false;
        public int QuestionId { get; set; }

        //NAVIGATION PROPERTIES
        public Question Question { get; set; }
    }
}
