using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using UTSATSAPI.Models.Models;

namespace UTSATSAPI.Helpers.Common
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IConfiguration _configuration;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger,IConfiguration configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            
            var errorResponse = new ResponseObject
            {
                statusCode = StatusCodes.Status500InternalServerError
            };
            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid Token"))
                    {
                        response.StatusCode = StatusCodes.Status403Forbidden;
                        errorResponse.Message = ex.Message;
                        errorResponse.Details = ex.StackTrace;
                        break;
                    }
                    response.StatusCode = StatusCodes.Status400BadRequest;
                    errorResponse.Message = ex.Message;
                    errorResponse.Details = ex.StackTrace;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = exception.Message;
                    errorResponse.Details = exception.StackTrace;
                    break;
            }
            _logger.LogError(exception.Message);
            SendEmails sendEmails = new SendEmails();
            var result = JsonSerializer.Serialize(errorResponse);

            EmailBinder binder = new EmailBinder(_configuration,null);
            List<string> toemail = new List<string>() { "parth.mehta@uplers.in" , "himani.s@uplers.in" };
            List<string> toemailname = new List<string>() { "ParthMehta" , "Himani" };
            binder.BindEmailForError(toemail, toemailname, "Exception Email", "<b> [ Error ] </b><br>" + result);
            
            await context.Response.WriteAsync(result);
        }
    }
}
