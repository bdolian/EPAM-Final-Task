namespace KnowledgeTestingSystemDAL.Entities
{
    public class QuestionOption : BaseEntity
    {
        public int QuestionId { get; set; }
        public int OptionId { get; set; }
        public int CorrectOptionId { get; set; }

        //NAVIGATION PROPERTIES
        public Question Question { get; set; }
        public Option Option { get; set; }
    }
}
