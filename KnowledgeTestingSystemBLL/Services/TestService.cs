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
            testToCreate.TimeToEnd = int.Parse(test.TimeToEnd);
            await _unitOfWork.TestRepository.AddAsync(testToCreate);
        }

        public async Task<Result> CheckTestAsync(PassedTest test)
        {
            if (test == null) throw new ArgumentNullException(nameof(test));

            var testInfo = await _unitOfWork.TestRepository.GetByIdAsync(test.TestId);
            if (testInfo is null) throw new Exception("There is no such test");

            testInfo.Questions = (await _unitOfWork.QuestionRepository.GetByTestIdAsync(test.TestId)).ToList();
            foreach(var question in testInfo.Questions)
            {
                question.Options = (await _unitOfWork.OptionRepository.GetByQuestionIdAsync(question.Id)).ToList();
            }
            QuestionAnswer[] correctAnswers = new QuestionAnswer[testInfo.Questions.Count];
            for(int i = 0; i < testInfo.Questions.Count; i++)
            {
                for(int j = 0; j < testInfo.Questions.ElementAt(i).Options.Count; j++)
                {
                    if (testInfo.Questions.ElementAt(i).Options.ElementAt(j).IsCorrect)
                    {
                        correctAnswers[i] = new QuestionAnswer();
                        correctAnswers[i].QuestionId = testInfo.Questions.ElementAt(i).Id;
                        correctAnswers[i].AnswerId = testInfo.Questions.ElementAt(i).Options.ElementAt(j).Id;
                    }
                }
            }
            Array.Sort(test.QuestionAnswers, 
                delegate(QuestionAnswer x, QuestionAnswer y) 
                { 
                    return x.QuestionId.CompareTo(y.QuestionId); 
                });

            Result result = new Result();
            for(int i = 0; i < correctAnswers.Length; i++)
            {
                if (test.QuestionAnswers[i].AnswerId == correctAnswers[i].AnswerId)
                    result.Grade++;
            }
            result.QuestionAnswers = correctAnswers;
            result.TestId = test.TestId;

            return result;
        }
        public async Task<bool> DeleteAsync(int id)
        {  
            var testToDelete = await _unitOfWork.TestRepository.GetByIdAsync(id);

            if (testToDelete == null)
                return false;

            testToDelete.Questions = (await _unitOfWork.QuestionRepository.GetByTestIdAsync(id)).ToList();

            foreach(var question in testToDelete.Questions)
            {
                question.Options = (await _unitOfWork.OptionRepository.GetByQuestionIdAsync(question.Id)).ToList();
                foreach(var option in question.Options)
                {
                    await _unitOfWork.OptionRepository.DeleteByIdAsync(option.Id);
                }
                await _unitOfWork.QuestionRepository.DeleteByIdAsync(question.Id);
            }
            await _unitOfWork.TestRepository.DeleteByIdAsync(id);

            return true;
        }

        public async Task<TestDTO> EditAsync(TestDTO test)
        {
            if (test is null) throw new ArgumentNullException(nameof(test));

            var parsedTest = ParseTest(test);
            await _unitOfWork.TestRepository.UpdateAsync(parsedTest);
            foreach(var question in parsedTest.Questions)
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
            var dbTest = await _unitOfWork.TestRepository.GetByIdAsync(id);
            if (dbTest == null) throw new Exception("There is no such test");

            var test = _mapper.Map<Test, TestDTO>(dbTest);
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
            testToCreate.Questions = testToCreate.Questions.ToList();
            for (int i = 0; i < testToCreate.NumberOfQuestions; i++)
            {
                testToCreate.Questions.ElementAt(i).NumberOfOptions = testToCreate.Questions.ElementAt(i).Options.Count;
                testToCreate.Questions.ElementAt(i).Options = testToCreate.Questions.ElementAt(i).Options.ToList();
            }

            return testToCreate;
        }
    }
}
