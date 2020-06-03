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
        public IActionResult Index()
        {
            // need to validate before generating the claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,"userId"),
                new Claim("Email","testEmail")
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
                ) ;
            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { access_token= tokenJson });
        }
    }
}