using KnowledgeTestingSystemDAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private IOptionRepository _optionRepository;
        private IQuestionRepository _questionRepository;
        private ITestRepository _testRepository;
        private IUserProfileRepository _userProfileRepository;
        private IUserRepository _userRepository;
        private IUserProfileTestRepository _userProfileTestRepository;
        private IQuestionOptionRepository _questionOptionRepository;
        private ITestQuestionRepository _testQuestionRepository;

        public IOptionRepository OptionRepository
        {
            get
            {
                if (_optionRepository == null) { throw new NullReferenceException(); }
                return _optionRepository;
            }
        }
        public IQuestionRepository QuestionRepository
        {
            get
            {
                if (_questionRepository == null) { throw new NullReferenceException(); }
                return _questionRepository;
            }

        }
        public ITestRepository TestRepository
        {
            get
            {
                if (_testRepository == null) { throw new NullReferenceException(); }
                return _testRepository;
            }

        }
        public IUserProfileRepository UserProfileRepository
        {
            get
            {
                if (_userProfileRepository == null) { throw new NullReferenceException(); }
                return _userProfileRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null) { throw new NullReferenceException(); }
                return _userRepository;
            }
        }
        public IUserProfileTestRepository UserProfileTestRepository
        {
            get
            {
                if (_userProfileTestRepository == null) { throw new NullReferenceException(); }
                return _userProfileTestRepository;
            }
        }
        public IQuestionOptionRepository QuestionOptionRepository
        {
            get
            {
                if (_questionOptionRepository == null) { throw new NullReferenceException(); }
                return _questionOptionRepository;
            }
        }
        public ITestQuestionRepository TestQuestionRepository
        {
            get
            {
                if (_testQuestionRepository == null) { throw new NullReferenceException(); }
                return _testQuestionRepository;
            }
        }

        Task<int> IUnitOfWork.SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
