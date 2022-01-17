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
        /// <summary>
        /// This method parses the test using this.ParseTest() method and adds it into a DB 
        /// </summary>
        /// <param name="test">Test to create model</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Model is null</exception>
        public async Task CreateAsync(TestDTO test)
        {
            if(test == null)
                throw new ArgumentNullException(nameof(test));

            var testToCreate = ParseTest(test);
            testToCreate.TimeToEnd = int.Parse(test.TimeToEnd);
            await _unitOfWork.TestRepository.AddAsync(testToCreate);
        }

        /// <summary>
        /// This method checks the test that user has passed. Firstly it gets all test info from DB by test id, 
        /// then compare correct answers with the given ones.
        /// </summary>
        /// <param name="test">Passed test model</param>
        /// <returns>Result model (grade and correct answers)</returns>
        /// <exception cref="ArgumentNullException">Model is null</exception>
        /// <exception cref="Exception">There is no such test</exception>
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
                        correctAnswers[i] = new QuestionAnswer
                        {
                            QuestionId = testInfo.Questions.ElementAt(i).Id,
                            AnswerId = testInfo.Questions.ElementAt(i).Options.ElementAt(j).Id
                        };
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

        /// <summary>
        /// This method deletes all test information(Firstly all options, then all questions, then the test itself)
        /// </summary>
        /// <param name="id">Test id</param>
        /// <returns>True if everything is fine, false otherwise</returns>
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

        /// <summary>
        /// This method edits the test based on passed model
        /// </summary>
        /// <param name="test">New test model</param>
        /// <returns>New test</returns>
        /// <exception cref="ArgumentNullException">Model is null</exception>
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

        /// <summary>
        /// This method returns all elements based on the filter(some condition)
        /// </summary>
        /// <param name="filter">Condition for the test</param>
        /// <returns>All tests that corresponds the condition</returns>
        public async Task<IEnumerable<TestDTO>> GetAsync(Func<TestDTO, bool> filter)
        {
            var tests = _mapper.Map<IEnumerable<Test>, IEnumerable<TestDTO>>(await _unitOfWork.TestRepository.GetAllAsync());

            return tests.Where(filter).ToList();
        }

        /// <summary>
        /// This method returns all information about the test based on its id
        /// </summary>
        /// <param name="id">Test id</param>
        /// <returns>Test</returns>
        /// <exception cref="Exception">There is no such test</exception>
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

        /// <summary>
        /// This method parses the test from TestDTO(BLL) to Test(DAL) and counts number of questions and options for each of them.
        /// </summary>
        /// <param name="test">Test to parse</param>
        /// <returns>Parsed test</returns>
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
