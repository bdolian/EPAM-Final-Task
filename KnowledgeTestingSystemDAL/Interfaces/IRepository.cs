using KnowledgeTestingSystemDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T CreateAsync(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        T UpdateAsync(T entity);
        bool DeleteAsync(T entity);
        Task DeleteByIdAsync(int id);

    }
}
