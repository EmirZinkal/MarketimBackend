using Business.Abstract;
using Entities.Dtos;
using Entities.Dtos.Auth;
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

            if (accessTokenResult.Success)
                return Ok(accessTokenResult.Data);

            return BadRequest(accessTokenResult.Message);
        }

        [HttpPost("login")]
        public IActionResult Login(UserForLoginDto loginDto)
        {
            var loginResult = _authService.Login(loginDto);
            if (!loginResult.Success)
                return BadRequest(loginResult.Message);

            var accessTokenResult = _authService.CreateAccessToken(loginResult.Data);
            if (accessTokenResult.Success)
                return Ok(accessTokenResult.Data);
            return BadRequest(accessTokenResult.Message);
        }

        [HttpPost("forgot-password")]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordRequestDto request)
        {
            var result = _authService.SendPasswordResetEmail(request);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        [HttpPost("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequestDto request)
        {
            var result = _authService.ResetPassword(request);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }
    }
}
