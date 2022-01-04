using KnowledgeTestingSystemDAL.Interfaces;
using KnowledgeTestingSystemDAL.Repositories;
using System;

namespace KnowledgeTestingSystemDAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private KnowledgeTestingSystemDbContext _database;
        private IOptionRepository _optionRepository;
        private IQuestionRepository _questionRepository;
        private ITestRepository _testRepository;
        private IUserProfileRepository _userProfileRepository;
        private IUserRepository _userRepository;
        private IUserProfileTestRepository _userProfileTestRepository;
        private IQuestionOptionRepository _questionOptionRepository;
        private ITestQuestionRepository _testQuestionRepository;

        public UnitOfWork(KnowledgeTestingSystemDbContext database)
        {
            _database = database;
        }

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
                if (_testRepository == null) { _testRepository = new TestRepository(_database); }
                return _testRepository;
            }

        }
        public IUserProfileRepository UserProfileRepository
        {
            get
            {
                if (_userProfileRepository == null) { _userProfileRepository = new UserProfileRepository(_database); }
                return _userProfileRepository;
            }
        }
        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null) { _userRepository = new UserRepository(_database); }
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

        async void IUnitOfWork.SaveAsync()
        {
            await _database.SaveChangesAsync();
        }
    }
}
