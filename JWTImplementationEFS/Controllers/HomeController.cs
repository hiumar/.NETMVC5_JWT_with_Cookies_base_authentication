using JWTImplementationEFS.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace JWTImplementationEFS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private JwtSecurityToken GetToken(User user)
        {
            var claims = new[] {
                new Claim("employeeid",user.employeeId),
                new Claim(ClaimTypes.Name, user.USerName),
                new Claim(ClaimTypes.Email, user.Email), 
                new Claim(ClaimTypes.Role, user.Role)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretkey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "www.joydipkanjilal.net",
                audience: "http://localhost:36145/",
                expires: DateTime.Now.AddMinutes(1),
                claims: claims,
                signingCredentials: creds);

            return token;

        }

        private ClaimsPrincipal ValidateJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("mysecretkey");

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return new ClaimsPrincipal((IEnumerable<ClaimsIdentity>)validatedToken);
            }
            catch
            {
                // return null if the token is invalid
                return null;
            }
        }
    }
}