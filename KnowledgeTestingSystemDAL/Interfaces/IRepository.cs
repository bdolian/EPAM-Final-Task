using KnowledgeTestingSystemDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteByIdAsync(int id);

    }
}
