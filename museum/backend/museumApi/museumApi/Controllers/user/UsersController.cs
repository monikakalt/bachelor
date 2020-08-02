using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using museum.Application.helpers;
using museum.Application.main;
using museum.Application.main.users.dto;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace museumApi.Controllers.user
{
    [Authorize]
    [Route("/api/[controller]")]
    public class UsersController : Controller
    {
        protected readonly IUsersService _usersService;
        private readonly AppSettings _appSettings;

        public UsersController(IUsersService usersService, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            _usersService = usersService;
        }
        // GET api/values
        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = _usersService.GetAllUsers();
            return Ok(users);

        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _usersService.GetUserById(id);
                if (user == null)
                    return NotFound();
                return new ObjectResult(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return BadRequest();

        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserDTO userDto)
        {
            var user = _usersService.Authenticate(userDto.Email, userDto.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.UserData, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                IsAdmin = user.IsAdmin,
                Token = tokenString
            });
        }



        [AllowAnonymous]
        [HttpPost]
        public IActionResult Save([FromBody] UserDTO user)
        {
            try
            {
                // save 
                _usersService.Save(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }


        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] UserDTO userDto)
        {
            try
            {
                // save 
                _usersService.UpdateUser(userDto, userDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

    }
}