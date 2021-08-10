using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Simple_API.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Simple_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        //Using to access appsettings variables
        private readonly IConfiguration _configuration;

        //Using to access database
        private readonly AppDBContext _context;

        public TokenController(IConfiguration configuration, AppDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {

            if (userInfo != null && userInfo.Email != null && userInfo.Password != null)
            {
                //Getting user from database
                var user = await GetUser(userInfo.Email, userInfo.Password);

                if(user != null)
                {
                    //JWT uses Claim to pass data through token
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]), //{sub: (subject key from appsettings)}
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),     //{jti: (unique identifier)}
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),    //{iat: 8/6/2021 1:43:31 PM}
                        new Claim("UserId", user.UserId.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("Password", user.Password),
                        new Claim("Privileges", user.Privileges.ToString())
                    };

                    //Converting key from appsettings to array of bytes
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    //Encoding with HmacSha256
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signIn
                    );


                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));

                } else {

                    return BadRequest("Invalid credentials");

                }

            } else {

                return BadRequest();

            }
        }

        [HttpGet]
        public async Task<UserInfo> GetUser(string email, string password)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

    }
}
