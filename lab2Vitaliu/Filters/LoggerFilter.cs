using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lab2Vitaliu.Filters
{
    public class LoggerFilter : Attribute, IResourceFilter
    {
        ILogger _logger;
        public LoggerFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("LoggerFilter");
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _logger.LogInformation($"OnResourceExecuted - {DateTime.Now} : {context.ActionDescriptor.DisplayName}");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _logger.LogInformation($"OnResourceExecuting - {DateTime.Now} : {context.ActionDescriptor.DisplayName}");
        }
    }
}
