using KnowledgeTestingSystem.Filters;
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
            return Ok(await _testService.GetByIdAsync(id));
        }

        [HttpPost("createTest")]
        public async Task<IActionResult> CreateTest(TestDTO test)
        {
            await _testService.CreateAsync(test);

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
        public async Task<IActionResult> EditTest(TestDTO newTest)
        {
            if (newTest == null)
                throw new ArgumentNullException(nameof(newTest));

            await _testService.EditAsync(newTest);

            return NoContent();
        }

        [HttpPost("passTest")]
        public async Task<IActionResult> PassTest(PassedTest test)
        { 
            var result = await _testService.CheckTestAsync(test);
            return Ok(result);
        }

    }
}
