using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace KnowledgeTestingSystem.Filters
{
    public class LoggerActionFilterAttribute : IActionFilter
    {
        private readonly ILogger<LoggerActionFilterAttribute> _logger;

        public LoggerActionFilterAttribute(ILogger<LoggerActionFilterAttribute> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("\n" + context.HttpContext.Request.Path + " " + context.HttpContext.Request.Method + " Executed\n");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("\n" + context.HttpContext.Request.Path + " " + context.HttpContext.Request.Method + " Executing\n");
        }
    }
}
