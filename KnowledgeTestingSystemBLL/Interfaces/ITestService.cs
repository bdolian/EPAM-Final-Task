using KnowledgeTestingSystemBLL.Entities;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Interfaces
{
    public interface ITestService : IService<TestDTO>
    {
        Task<TestDTO> GetByIdAsync(int id);
    }
}
