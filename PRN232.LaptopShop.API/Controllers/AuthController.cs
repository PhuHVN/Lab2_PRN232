using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PRN232.LaptopShop.Service;
using PRN232.LaptopShop.Service.DTO;

namespace PRN232.LaptopShop.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest loginRequest)
        {
            var token = await authService.LoginEmail(loginRequest);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new { Token = token });
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AccountRequest registerRequest)
        {
            var result = await authService.Register(registerRequest);
            return Ok("Registration successful.");
        }
    }
}
