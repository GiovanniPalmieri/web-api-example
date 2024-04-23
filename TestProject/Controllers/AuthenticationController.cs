using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TestProject.Controllers {
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase {

        private readonly IConfiguration _configuration;

        public AuthenticationController(IConfiguration configuration) {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody) {
            var user = ValidateUserCredentials(authenticationRequestBody.Username, 
                authenticationRequestBody.Password, authenticationRequestBody.Age);

            if (user == null) {
                return Unauthorized();
            }

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(_configuration["Authentication:SecretForKey"] 
                ?? throw new Exception()));

            var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim> {
                new ("sub", user.UserId.ToString()),
                new ("fullName", user.Fullname.ToString()),
                new ("age", user.Age.ToString() )
            };

            var jwtSecurityToken = new JwtSecurityToken(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);

        }

        private EmployUser ValidateUserCredentials(string? username, string? password, int? age) {

            //assuming credential are valid
            return new EmployUser(1, username ?? throw new Exception(), "Utente", age ?? throw new Exception());

        }

        public class AuthenticationRequestBody {
            public string? Username { get; set; }
            public string? Password { get; set; }
            public int? Age { get; set; }
        }

        private class EmployUser {
            public int UserId { get; }
            public string Username { get; }
            public string Fullname { get; }
            public int Age { get;  }

            public EmployUser(int userId, string username, string fullname, int age) {
                UserId = userId;
                Username = username;
                Fullname = fullname;
                Age = age;
            }
        }
    }
}
