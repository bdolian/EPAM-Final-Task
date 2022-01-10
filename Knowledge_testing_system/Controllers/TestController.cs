using AutoMapper;
using KnowledgeTestingSystem.Filters;
using KnowledgeTestingSystem.Models;
using KnowledgeTestingSystemBLL.Entities;
using KnowledgeTestingSystemBLL.Entities.DTO;
using KnowledgeTestingSystemBLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> DeleteTest(TestModel test)
        {
            var testToDelete = _mapper.Map<TestModel, TestDTO>(test);
            var isDeleted = await _testService.DeleteAsync(testToDelete);

            if (!isDeleted)
                throw new ArgumentException("You passed invalid test, it is not deleted");

            return Ok();
        }

        [HttpPut("editTest")]
        public async Task<IActionResult> EditTest(TestModel newTest)
        {
            if (newTest == null)
                throw new ArgumentNullException(nameof(newTest));

            var testToEdit = _mapper.Map<TestModel, TestDTO>(newTest);

            await _testService.EditAsync(testToEdit);

            return NoContent();
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
        
    }
}
