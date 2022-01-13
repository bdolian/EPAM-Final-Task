using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Test : BaseEntity
    {
        public int NumberOfQuestions { get; set; }
        public int TimeToEnd { get; set; } //IN MINUTES
        public string Name { get; set; }

        //NAVIGATION PROPERTIES
        public ICollection<UserProfileTest> UserProfiles { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
