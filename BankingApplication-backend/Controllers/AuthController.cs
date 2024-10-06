using BankingApplication_backend.DTOs;
using BankingApplication_backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
