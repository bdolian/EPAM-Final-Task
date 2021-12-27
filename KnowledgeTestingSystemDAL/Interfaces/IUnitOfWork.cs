using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL.Interfaces
{
    public interface IUnitOfWork
    {
        IOptionRepository OptionRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        ITestRepository TestRepository { get; }
        IUserProfileRepository UserProfileRepository { get; }
        IUserRepository UserRepository { get; }

        Task<int> SaveAsync();

    }
}
