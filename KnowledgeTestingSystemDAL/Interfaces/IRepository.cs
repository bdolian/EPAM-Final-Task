using KnowledgeTestingSystemDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteByIdAsync(int id);

    }
}
