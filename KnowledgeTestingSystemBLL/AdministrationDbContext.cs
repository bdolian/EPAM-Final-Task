using KnowledgeTestingSystemBLL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeTestingSystemBLL
{
    public class AdministrationDbContext : IdentityDbContext<ApplicationUser>
    {
        public AdministrationDbContext(DbContextOptions<AdministrationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(new[]
            {
                new IdentityRole("user")
                {
                    NormalizedName = "USER"
                },
                new IdentityRole("admin")
                {
                    NormalizedName = "ADMIN"
                }
            }); 
        }
    }
}
