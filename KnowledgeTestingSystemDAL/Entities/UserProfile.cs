using System;
using System.Collections.Generic;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class UserProfile : BaseEntity
    {
        public DateTime DateOfBirth { get; set; }
        public int UserId { get; set; }

        //NAVIGATION PROPERTIES
        public User User { get; set; }
        public ICollection<UserProfileTest> Tests { get; set; }
    }
}
