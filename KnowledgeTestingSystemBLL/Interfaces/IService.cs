using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface IService<T> where T : class
    {
        Task CreateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAsync(Func<T, bool> filter);
        Task<T> EditAsync(T test);
        Task<bool> DeleteAsync(T test);
    }
}
