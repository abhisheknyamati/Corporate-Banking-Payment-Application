using BankingApplication_backend.DTOs;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

namespace BankingApplication_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
            
        }
        [HttpPost]
        public async Task<string> Post([FromBody] CredDto value)
        {
            return await _authService.Login(value);
        }

        [HttpPost("request-password-reset")]
        public async Task<IActionResult> RequestPasswordReset([FromBody] PasswordResetRequestDto passwordResetRequestDto)
        {
            await _authService.GeneratePasswordResetToken(passwordResetRequestDto.Email);
            return Ok(new ResponseDto { Success = true, Message = "Reset token sent to email" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDto resetDto)
        {
            var result = await _authService.ResetPassword(resetDto.Token, resetDto.NewPassword);
            if (result)
                return Ok(new ResponseDto { Success = true, Message = "Password reset successful" });
            else
                return BadRequest(new ResponseDto { Success = false, Message = "Invalid or expired token" });
        }
    }
}
