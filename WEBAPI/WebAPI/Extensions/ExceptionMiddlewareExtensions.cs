using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;
using Entities.ErrorModel;
using Entities.Exceptions;

namespace WebAPI.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerService logger)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (exceptionFeature != null)
                    {
                        context.Response.StatusCode = exceptionFeature.Error switch
                        {
                            NotFoundException => (int)HttpStatusCode.NotFound,
                            ArgumentNullException => (int)HttpStatusCode.BadRequest,
                            _ => (int)HttpStatusCode.InternalServerError
                        };

                        logger.LogError($"Something went wrong : {exceptionFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = exceptionFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }

    }
}
