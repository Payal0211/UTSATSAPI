namespace UTSATSAPI.Helpers.Common
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UTSATSAPI.Models.Models;

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _appSettings;

        public JwtMiddleware(RequestDelegate next, IConfiguration appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task Invoke(HttpContext context, InMemoryDbContext _dbcontext)
        {

            GC.Collect(2, GCCollectionMode.Forced, true);
            GC.WaitForPendingFinalizers();
             
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var varIsTokenActive= await _dbcontext.JwtTokenDataModels.Where(x => x.JwtToken == token).FirstOrDefaultAsync().ConfigureAwait(false);
                    if(varIsTokenActive!=null)
                    {
                        attachUserToContext(context, token);
                    }
                }

            await _next(context);
            
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings["JWT:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "email").Value;
                var loginUserId = jwtToken.Claims.First(x => x.Type == "LoginUserId").Value;
                var loginUserTypeId = jwtToken.Claims.First(x => x.Type == "LoginUserTypeId").Value;

                SessionValues.LoginUserId = Convert.ToInt64(loginUserId);
                SessionValues.LoginUserTypeId = Convert.ToInt32(loginUserTypeId);

                // attach user to context on successful jwt validation
                context.Items["User"] = userId;
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
