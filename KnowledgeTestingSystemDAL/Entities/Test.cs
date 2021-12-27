using System;
using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class Test : BaseEntity
    {
        public int NumberOfQuestions { get; set; }
        public DateTime TimeToEnd { get; set; }
        public ICollection<UserProfileTest> UserProfiles { get; set; }
    }
}
