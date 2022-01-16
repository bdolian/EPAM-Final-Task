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
    [Authorize(Roles = "admin")]
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
            try
            {
                return Ok(await _testService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
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
                return BadRequest("You passed invalid test, it is not deleted");

            return Ok();
        }

        [HttpPut("editTest")]
        public async Task<IActionResult> EditTest(TestDTO newTest)
        {
            try
            {
                await _testService.EditAsync(newTest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost("passTest")]
        [AllowAnonymous]
        public async Task<IActionResult> PassTest(PassedTest test)
        {
            Result result = new Result();
            try
            {
                result = await _testService.CheckTestAsync(test);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(result);
        }

    }
}
