using Entities.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Services.Contracts;
using System;

namespace Presentation.ActionFilters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILoggerService _logger;

        public LogFilterAttribute(ILoggerService logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var logMessage = CreateLogDetails("OnActionExecuting", context.RouteData).ToString();
            _logger.LogInfo(logMessage);
        }

        private LogDetails CreateLogDetails(string modelName, RouteData routeData)
        {
            var logDetails = new LogDetails
            {
                ModelName = modelName,
                Controler = routeData.Values["controller"]?.ToString(),
                Action = routeData.Values["action"]?.ToString(),
                Id = routeData.Values.ContainsKey("id") ? routeData.Values["id"]?.ToString() : null
            };

            return logDetails;
        }
    }
}
