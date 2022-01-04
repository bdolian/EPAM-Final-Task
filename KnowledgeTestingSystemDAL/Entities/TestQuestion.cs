namespace KnowledgeTestingSystemDAL.Entities
{
    public class TestQuestion : BaseEntity
    {
        public int QuestionId { get; set; }
        public int TestId { get; set; }

        //NAVIGATION PROPERTIES
        public Test Test { get; set; }
        public Question Question { get; set; }
    }
}
