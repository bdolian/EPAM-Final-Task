using System.ComponentModel.DataAnnotations;

namespace KnowledgeTestingSystemDAL.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set;}
        public string Email { get; set; }

        //NAVIGATION PROPERTIES
        public UserProfile UserProfile { get; set; }
    }
}
