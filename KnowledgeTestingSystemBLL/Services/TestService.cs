using AutoMapper;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Interfaces;
using KnowledgeTestingSystemDAL.Entities;
using KnowledgeTestingSystemDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KnowledgeTestingSystemBLL.Services
{
    public class TestService : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Test, TestDTO>();
                cfg.CreateMap<TestDTO, Test>();
            });
            _mapper = new Mapper(config);
        }
        public async Task CreateAsync(TestDTO test)
        {
            if(test == null)
                throw new ArgumentNullException(nameof(test));

            await _unitOfWork.TestRepository.AddAsync(new Test
            {
                Name = test.Name,
                NumberOfQuestions = test.NumberOfQuestions,
                TimeToEnd = test.TimeToEnd
            });
        }

        public async Task<bool> DeleteAsync(TestDTO test)
        {
            var testToDelete = await _unitOfWork.TestRepository.GetByIdAsync(test.Id);

            if (testToDelete == null)
                return false;

            await _unitOfWork.TestRepository.DeleteByIdAsync(test.Id);

            return true;
        }

        public async Task<TestDTO> EditAsync(TestDTO test)
        {
            var mappedTest = _mapper.Map<TestDTO, Test>(test);
            await _unitOfWork.TestRepository.UpdateAsync(mappedTest);

            var updatedTest = await _unitOfWork.TestRepository.GetByIdAsync(test.Id);
            var resultEntity = _mapper.Map<Test, TestDTO>(updatedTest);
            return resultEntity;
        }

        public async Task<IEnumerable<TestDTO>> GetAllAsync()
        {
            var users = _mapper.Map<IEnumerable<Test>, IEnumerable<TestDTO>>(await _unitOfWork.TestRepository.GetAllAsync());

            return users;
        }

        public async Task<IEnumerable<TestDTO>> GetAsync(Func<TestDTO, bool> filter)
        {
            var users = _mapper.Map<IEnumerable<Test>, IEnumerable<TestDTO>>(await _unitOfWork.TestRepository.GetAllAsync());

            return users.Where(filter).ToList();
        }

    }
}
