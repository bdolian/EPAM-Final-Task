using KnowledgeTestingSystemBLL.Entities.DTO;

namespace KnowledgeTestingSystemBLL.Entities
{
    public class UserCompleteInformation
    {
        public UserDTO User { get; set; }
        public UserProfileDTO UserProfile { get; set; }
        public UserProfileTestDTO[] UserProfileTest { get; set; } = null;
    }
}
