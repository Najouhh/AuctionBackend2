using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AuktionBackend.Repository.Repos;
using AuktionBackend.Models.Entities;
using AuktionBackend.Repository.Interfaces;
using AuktionBackend.Models.DTOS;

namespace AuktionBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InloggningController : ControllerBase
    {
        private readonly IUserRepo _UserRepo;
        public InloggningController(IUserRepo UserRepo)
        {

            _UserRepo = UserRepo;
        }

        [HttpPost("Signin")]
        public IActionResult Login(UserPostDTO user)
        {
            if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid input");
            }
            var loginuser = _UserRepo.Login(user.Username, user.Password);
            if (loginuser != null && loginuser.Password == user.Password)
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, loginuser.UserId.ToString()));
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysecretKey123456789101112131415!#"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                            issuer: "http://localhost:7290/",
                            audience: "http://localhost:7290/",
                            claims: claims,
                            expires: DateTime.Now.AddMinutes(20),
                            signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { Token = tokenString });
            }

            return Unauthorized("Invalid credentials");
        }
    }

}


