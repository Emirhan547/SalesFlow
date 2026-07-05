using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.AuthDtos;
using SalesFlow.Business.Services.AuthServices;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthsController(IAuthService authService)
        {
            _authService = authService;
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            int userId = int.Parse(User.FindFirst("sub")!.Value);

            var result = await _authService.LogoutAsync(userId);

            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}