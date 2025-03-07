using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UTSATSAPI.Middlewares
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _requiredHeaderKey;
        private readonly string _requiredHeaderValue;

        public CustomAuthorizeAttribute(string requiredHeaderKey, string requiredHeaderValue)
        {
            _requiredHeaderKey = requiredHeaderKey;
            _requiredHeaderValue = requiredHeaderValue;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the header exists
            if (context.HttpContext.Request.Headers.TryGetValue(_requiredHeaderKey, out var headerValue))
            {
                // Validate the header value
                if (headerValue != _requiredHeaderValue)
                {
                    // If the header value is incorrect, return 403 Forbidden
                    context.Result = new UnauthorizedResult();
                }
            }
            else
            {
                // If the header is missing, return 401 Unauthorized
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
