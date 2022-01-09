using KnowledgeTestingSystem.Filters;
using KnowledgeTestingSystem.Models;
using KnowledgeTestingSystemBLL.Entities;
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

        public TestController(ITestService testService)
        {
            _testService = testService;
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
            return Ok(await _testService.GetAsync(x => x.Id == id));
        }

        [HttpPost("createTest")]
        public async Task<IActionResult> CreateTest(TestModel test)
        {
            await _testService.CreateAsync(new TestDTO
            {
                Name = test.Name,
                NumberOfQuestions = test.NumberOfQuestions,
                TimeToEnd = test.TimeToEnd
            });
            return Ok();
        }

        [HttpDelete("deleteTest")]
        public async Task<IActionResult> DeleteTest(TestModel test)
        {
            var isDeleted = await _testService.DeleteAsync(new TestDTO
            {
                Id = test.Id,
                Name = test.Name,
                NumberOfQuestions = test.NumberOfQuestions,
                TimeToEnd = test.TimeToEnd
            });

            if (!isDeleted)
                throw new ArgumentException("You passed invalid test, it is not deleted");

            return Ok();
        }

        [HttpPut("editTest")]
        public async Task<IActionResult> EditTest(TestModel newTest)
        {
            if (newTest == null)
                throw new ArgumentNullException(nameof(newTest));

            await _testService.EditAsync(new TestDTO
            {
                Id = newTest.Id,
                Name = newTest.Name,
                TimeToEnd = newTest.TimeToEnd,
                NumberOfQuestions = newTest.NumberOfQuestions
            });

            return NoContent();
        }
        
    }
}
