using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace JWTImplementationEFS.App_Start
{

        public class JwtAuthenticationAttribute : AuthorizationFilterAttribute
        {
            private const string Secret = "mysecretkey"; // Replace with your secret key
       
        public override void OnAuthorization(HttpActionContext actionContext)
            {
                var token = ExtractToken(actionContext);
                if (token == null)
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                    return;
                }

                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(Secret);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime=true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var employeeId = jwtToken.Claims.First(x => x.Type == "employeeid").Value;

                    // Add the username to the request context so that it can be accessed in the controller action
                    actionContext.RequestContext.Principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim("employeeid", employeeId) }));
                }
                catch (Exception)
                {
                    actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                }
            }

            private static string ExtractToken(HttpActionContext actionContext)
            {
                var authorizationHeader = actionContext.Request.Headers.Authorization;
                if (authorizationHeader == null || !authorizationHeader.Scheme.Equals("bearer", StringComparison.OrdinalIgnoreCase))
                {
                    return null;
                }

                return authorizationHeader.Parameter;
            }
        }
    }

