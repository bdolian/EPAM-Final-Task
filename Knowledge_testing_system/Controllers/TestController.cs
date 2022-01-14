using AutoMapper;
using KnowledgeTestingSystem.Filters;
using KnowledgeTestingSystem.Models;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Entities.DTO;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KnowledgeTestingSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ModelStateActionFilter]
    [Authorize(Roles = "user, admin")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;
        private readonly IMapper _mapper;

        public TestController(ITestService testService)
        {
            _testService = testService;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OptionModel, OptionDTO>().ReverseMap();
                cfg.CreateMap<QuestionModel, QuestionDTO>().ReverseMap();
                cfg.CreateMap<TestModel, TestDTO>().ReverseMap();
                cfg.CreateMap<PassedTestModel, PassedTest>().ReverseMap();
                cfg.CreateMap<ResultModel, Result>().ReverseMap();
            });
            _mapper = new Mapper(config);
        }
        
        [HttpGet("getTests")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTests()
        {
            return Ok(await _testService.GetAllAsync());
        }

        [HttpGet("getTestById")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTestById(int id)
        {
            return Ok(await _testService.GetByIdAsync(id));
        }

        [HttpPost("createTest")]
        public async Task<IActionResult> CreateTest(TestModel test)
        {
            var testToCreate = ParseTestToDTO(test);
            await _testService.CreateAsync(testToCreate);

            return Ok();
        }

        [HttpDelete("deleteTest")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var isDeleted = await _testService.DeleteAsync(id);

            if (!isDeleted)
                throw new ArgumentException("You passed invalid test, it is not deleted");

            return Ok();
        }

        [HttpPut("editTest")]
        public async Task<IActionResult> EditTest(TestModel newTest)
        {
            if (newTest == null)
                throw new ArgumentNullException(nameof(newTest));

            var testToEdit = ParseTestToDTO(newTest);

            await _testService.EditAsync(testToEdit);

            return NoContent();
        }

        [HttpPost("passTest")]
        public async Task<IActionResult> PassTest(PassedTestModel test)
        { 
            var passedTest = ParsePassedTestModel(test);
            var result = ParseToResultModel(await _testService.CheckTestAsync(passedTest));
            return Ok(result);
        }

        private TestDTO ParseTestToDTO(TestModel test)
        {
            var testToCreate = _mapper.Map<TestModel, TestDTO>(test);
            testToCreate.NumberOfQuestions = test.Questions.Length;
            for(int i = 0; i < testToCreate.NumberOfQuestions; i++)
            {
                testToCreate.Questions[i].NumberOfOptions = testToCreate.Questions[i].Options.Length;
            }
            testToCreate.TimeToEnd = int.Parse(test.TimeToEnd);

            return testToCreate;
        }

        private PassedTest ParsePassedTestModel(PassedTestModel model)
        {
            PassedTest test = new PassedTest();

            test.TestId = model.TestId;
            Dictionary<int, int> questionAnswerDict = new Dictionary<int, int>();
            for(int i = 0; i < model.QuestionAnswers.Length; i++)
            {
                questionAnswerDict.Add(model.QuestionAnswers[i].QuestionId, model.QuestionAnswers[i].AnswerId);
            }
            test.QuestionAnswerKeyValue = questionAnswerDict;
            return test;
        }

        private ResultModel ParseToResultModel(Result resultInfo)
        {
            ResultModel model = new ResultModel();
            int i = 0;
            model.TestId = resultInfo.TestId;
            model.Grade = resultInfo.Grade;
            model.QuestionAnswers = new QuestionAnswerModel[resultInfo.QuestionAnswerKeyValue.Count];

            foreach(var item in resultInfo.QuestionAnswerKeyValue)
            {
                model.QuestionAnswers[i] = new QuestionAnswerModel();
                model.QuestionAnswers[i].AnswerId = item.Value;
                model.QuestionAnswers[i].QuestionId = item.Key;
                i++;
            }

            return model;
        }
        
    }
}
