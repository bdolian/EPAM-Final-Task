using KnowledgeTestingSystemDAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeTestingSystemDAL
{
    public class KnowledgeTestingSystemDbContext : DbContext
    {
        public KnowledgeTestingSystemDbContext(DbContextOptions<KnowledgeTestingSystemDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<UserProfileTest> UserProfilesTests { get; set; }

    }
}
