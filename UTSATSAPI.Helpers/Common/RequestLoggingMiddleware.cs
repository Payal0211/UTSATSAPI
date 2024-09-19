using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UTSATSAPI.Models.ComplexTypes;
using UTSATSAPI.Models.Models;
using UTSATSAPI.Models.ViewModels.Request_ResponseModels;
using static iTextSharp.text.pdf.AcroFields;

namespace UTSATSAPI.Helpers.Common
{
    public class RequestLoggingMiddleware
    {
        #region Variables
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        #endregion
        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, TalentConnectAdminDBContext _db)
        {
            string strLogRequest = await LogRequest(context);

            var originalResponseBody = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                try
                {
                    context.Response.Body = responseBody;
                    await _next.Invoke(context);

                    string strLogResponse = await LogResponse(context, responseBody, originalResponseBody);

                    var varStatusCode = context.Response?.StatusCode;
                    var varHostUrl = context.Request?.Host.Value;
                    var varApiPath = context.Request?.Path.Value;
                    long LoggedInUserId = SessionValues.LoginUserId;


                    bool filterApiPath = true;

                    if (varApiPath.Contains("/UserOperationsAPI/AdminLogin"))
                    {
                        filterApiPath = false;
                    }

                    if (!string.IsNullOrEmpty(strLogRequest) && !string.IsNullOrEmpty(strLogResponse) && varStatusCode!=200 && filterApiPath)
                    {
                        GenUtsreactApiRecord objGenUtsreactApiRecord = new();
                        
                        objGenUtsreactApiRecord.FromApiUrl = varHostUrl;
                        objGenUtsreactApiRecord.ToApiUrl = varApiPath;
                        objGenUtsreactApiRecord.PayloadToSend = strLogRequest.ToString();
                        objGenUtsreactApiRecord.ResponseReceived = strLogResponse.ToString();
                        objGenUtsreactApiRecord.StatusCode = varStatusCode;
                        objGenUtsreactApiRecord.CreatedById = LoggedInUserId;
                        objGenUtsreactApiRecord.CreatedByDateTime = DateTime.Now;
                        _db.GenUtsreactApiRecords.Add(objGenUtsreactApiRecord);
                        await _db.SaveChangesAsync();
                    }
                    await _next(context);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private async Task<string> LogRequest(HttpContext context)
        {
            var requestContent = new StringBuilder();

            requestContent.AppendLine(@"=== Request Info ===");
            requestContent.AppendLine($"method = {context.Request.Method.ToUpper()}");
            requestContent.AppendLine($"path = {context.Request.Path}");

            requestContent.AppendLine(@"-- headers");
            foreach (var (headerKey, headerValue) in context.Request.Headers)
            {
                requestContent.AppendLine($"header = {headerKey}    value = {headerValue}");
            }

            requestContent.AppendLine(@"-- body");
            context.Request.EnableBuffering();
            var requestReader = new StreamReader(context.Request.Body);
            var content = await requestReader.ReadToEndAsync();
            if (!string.IsNullOrEmpty(content))
            {
                requestContent.AppendLine($"body = {content}");

                //_logger.LogInformation(requestContent.ToString());
                context.Request.Body.Position = 0;
                return requestContent.ToString();
            }
            return null;
        }
        private async Task<string> LogResponse(HttpContext context, MemoryStream responseBody, Stream originalResponseBody)
        {
            var responseContent = new StringBuilder();
            responseContent.AppendLine(@"=== Response Info ===");

            responseContent.AppendLine(@"-- headers");
            foreach (var (headerKey, headerValue) in context.Response.Headers)
            {
                responseContent.AppendLine($"header = {headerKey}    value = {headerValue}");
            }

            responseContent.AppendLine(@"-- body");
            responseBody.Position = 0;
            var content = await new StreamReader(responseBody).ReadToEndAsync();
            if (!string.IsNullOrEmpty(content))
            {
                responseContent.AppendLine($"body = {content}");
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalResponseBody);
                context.Response.Body = originalResponseBody;

                //_logger.LogInformation(responseContent.ToString());
                return responseContent.ToString();
            }
            return null;
        }
    }
}
