using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.IdentityModel.JsonWebTokens;

namespace signOnApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if ((username != "admin") || (password != "admin123"))//We can also validate this by database.
            {
                return Ok("Invalid Username or Password!");
            }

            var tokenJson = GenerateToken();
            return Ok(new { access_token= tokenJson });
        }

        private string GenerateToken()
        {
            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub,"123"),
                new Claim("Email","testEmail@gmail.com")
            };
            var stringBytes = Encoding.UTF8.GetBytes(Constants.SecertKey);
            var key = new SymmetricSecurityKey(stringBytes);
            var algorithm = SecurityAlgorithms.HmacSha256;

            var signInCredentials = new SigningCredentials(key, algorithm);
            
            var token = new JwtSecurityToken(
                Constants.Issuer,
                Constants.Audience,
                claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signInCredentials
                );
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}