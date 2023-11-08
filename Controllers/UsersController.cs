using BCrypt.Net;
using LoginAuthDemo.DTO;
using LoginAuthDemo.Models;
using LoginAuthDemo.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IConfiguration _configuration;
        public UsersController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Register(UserDto userDto)
        {
            var existingUser = _userRepository.FindUser(userDto.UserName);
            if (existingUser == null)
            {
                if (_userRepository.AddUser(userDto) != null)
                    return Ok("User created Successfully.");
                return BadRequest("Some issue while inserting record");
            }
            return BadRequest("User already exist.");
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto userDto)
        {
            var existingUser = _userRepository.FindUser(userDto.UserName);
            if (existingUser != null)
            {
                if(BCrypt.Net.BCrypt.Verify(userDto.Password, existingUser.PasswordHash))
                {
                    var token = CreateToken(existingUser);
                    return Ok(token);
                }
                
            }
            return BadRequest("Username/Password does not match.");
        }

        private string CreateToken(User user)
        {
            var role = _userRepository.GetRoleName(user);
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
