using Business.Abstract;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserForRegisterDto registerDto)
        {
            var userExists = _authService.UserExists(registerDto.Email);
            if (!userExists.Success)
                return BadRequest(userExists.Message);

            var registerResult = _authService.Register(registerDto, registerDto.Password);
            var accessTokenResult = _authService.CreateAccessToken(registerResult.Data);

            return Ok(accessTokenResult);
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto loginDto)
        {
            var loginResult = _authService.Login(loginDto);
            if (!loginResult.Success)
                return BadRequest(loginResult.Message);

            var accessTokenResult = _authService.CreateAccessToken(loginResult.Data);
            return Ok(accessTokenResult);
        }
    }
}
