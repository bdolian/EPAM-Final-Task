using Microsoft.EntityFrameworkCore;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IKnowledgeTestingSystemDbContext
    {
        DbSet<T> Set<T>() where T : class;
    }
}
