using KnowledgeTestingSystemDAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IOptionRepository : IRepository<Option>
    {
        Task<IEnumerable<Option>> GetByQuestionIdAsync(int id);
    }
}
