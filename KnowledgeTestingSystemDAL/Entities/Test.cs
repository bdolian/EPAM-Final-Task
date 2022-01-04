using System;
using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Test : BaseEntity
    {
        public int NumberOfQuestions { get; set; }
        public DateTime TimeToEnd { get; set; }
        public string Name { get; set; }

        //NAVIGATION PROPERTIES
        public ICollection<UserProfileTest> UserProfiles { get; set; }
        public ICollection<TestQuestion> Questions { get; set; }
    }
}
