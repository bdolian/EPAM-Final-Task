namespace KnowledgeTestingSystemDAL.Entities
{
    public class UserProfileTest : BaseEntity
    {
        public int UserProfileId { get; set; }
        public int TestId { get; set; }
        public int Grade { get; set; }
        public int NumberOfAttempts { get; set; }

        //NAVIGATION PROPERTIES
        public Test Test { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}
