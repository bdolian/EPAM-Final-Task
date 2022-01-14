using AutoMapper;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Entities.DTO;
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
                cfg.CreateMap<Test, TestDTO>().ReverseMap();
                cfg.CreateMap<QuestionDTO, Question>().ReverseMap();
                cfg.CreateMap<OptionDTO, Option>().ReverseMap();
            });
            _mapper = new Mapper(config);
        }
        public async Task CreateAsync(TestDTO test)
        {
            if(test == null)
                throw new ArgumentNullException(nameof(test));

            var testToCreate = ParseTest(test);
            await _unitOfWork.TestRepository.AddAsync(testToCreate);
        }

        public async Task<Result> CheckTestAsync(PassedTest test)
        {
            if (test == null)
                throw new ArgumentNullException(nameof(test));

            var testInfo = await _unitOfWork.TestRepository.GetByIdAsync(test.TestId);
            testInfo.Questions = (await _unitOfWork.QuestionRepository.GetByTestIdAsync(test.TestId)).ToList();
            foreach(var question in testInfo.Questions)
            {
                question.Options = (await _unitOfWork.OptionRepository.GetByQuestionIdAsync(question.Id)).ToList();
            }
            var correctAnswers = new Dictionary<int, int>();
            for(int i = 0; i < testInfo.Questions.Count; i++)
            {
                for(int j = 0; j < testInfo.Questions.ElementAt(i).Options.Count; j++)
                {
                    if(testInfo.Questions.ElementAt(i).Options.ElementAt(j).IsCorrect)
                        correctAnswers.Add(testInfo.Questions.ElementAt(i).Id, 
                                           testInfo.Questions.ElementAt(i).Options.ElementAt(j).Id);
                }
            }
            Result result = new Result();
            foreach (var item in test.QuestionAnswerKeyValue)
            {
                if (item.Value == correctAnswers[item.Key])
                    result.Grade++;
            }
            result.QuestionAnswerKeyValue = correctAnswers;
            result.TestId = test.TestId;

            return result;
            
        }
        public async Task<bool> DeleteAsync(int id)
        {  
            var testToDelete = await _unitOfWork.TestRepository.GetByIdAsync(id);

            if (testToDelete == null)
                return false;

            await _unitOfWork.TestRepository.DeleteByIdAsync(id);

            return true;
        }

        public async Task<TestDTO> EditAsync(TestDTO test)
        {
            var mappedTest = _mapper.Map<TestDTO, Test>(test);
            await _unitOfWork.TestRepository.UpdateAsync(mappedTest);
            foreach(var question in mappedTest.Questions)
            {
                await _unitOfWork.QuestionRepository.UpdateAsync(question);
                foreach(var option in question.Options)
                {
                    await _unitOfWork.OptionRepository.UpdateAsync(option);
                }
            }

            var updatedTest = await _unitOfWork.TestRepository.GetByIdAsync(test.Id);
            var resultEntity = _mapper.Map<Test, TestDTO>(updatedTest);
            return resultEntity;
        }

        public async Task<IEnumerable<TestDTO>> GetAllAsync()
        {
            var tests = _mapper.Map<IEnumerable<Test>, IEnumerable<TestDTO>>(await _unitOfWork.TestRepository.GetAllAsync());

            return tests;
        }

        public async Task<IEnumerable<TestDTO>> GetAsync(Func<TestDTO, bool> filter)
        {
            var tests = _mapper.Map<IEnumerable<Test>, IEnumerable<TestDTO>>(await _unitOfWork.TestRepository.GetAllAsync());

            return tests.Where(filter).ToList();
        }

        public async Task<TestDTO> GetByIdAsync(int id)
        {
            var test = _mapper.Map<Test, TestDTO>(await _unitOfWork.TestRepository.GetByIdAsync(id));
            test.Questions = _mapper.Map<Question[], QuestionDTO[]>((await _unitOfWork.QuestionRepository.GetByTestIdAsync(id)).ToArray());
            for(int i = 0; i < test.Questions.Length; i++)
            {
                test.Questions[i].Options = _mapper.Map<Option[],OptionDTO[]>(
                    (await _unitOfWork.OptionRepository.GetByQuestionIdAsync(test.Questions[i].Id)).ToArray()
                    );
            }

            return test;
        }

        private Test ParseTest(TestDTO test)
        {
            var testToCreate = _mapper.Map<TestDTO, Test>(test);
            testToCreate.NumberOfQuestions = test.Questions.Length;
            for (int i = 0; i < testToCreate.NumberOfQuestions; i++)
            {
                testToCreate.Questions.ElementAt(i).NumberOfOptions = testToCreate.Questions.ElementAt(i).Options.Count;
            }

            return testToCreate;
        }
    }
}
