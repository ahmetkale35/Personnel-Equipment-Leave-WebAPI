using Entities.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Contracts;


namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        public readonly IServiceManager _service;
        public readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IServiceManager service, ILogger<AuthenticationController> logger)
        {
            _service = service;
            _logger = logger;
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegistirationDto userForRegistirationDto)
        {
            // Kullanıcı kaydı yap
            var result = await _service.Authentication.RegisterUser(userForRegistirationDto);
            // Eğer kayıt başarılı ise, 201 Created döndür
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    // Hata mesajlarını logla  
                    _logger.LogError($"Error Code: {error.Code}, Description: {error.Description}");
                } 
                return BadRequest(ModelState);
            }
            // Kayıt başarılı ise, 201 Created döndür 
            return StatusCode(201);
        }



        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForAuthDto)
        {
            // Kullanıcı doğrulama işlemi yap
            var result = await _service.Authentication.ValidateUser(userForAuthDto);
            // Eğer kullanıcı doğrulama başarılı ise, token oluştur
            if (!result)
            {
                _logger.LogWarning($"{nameof(Authenticate)} : Authentication failed. Wrong username or password.");
                return Unauthorized();
            }
            // Token oluştur ve döndür
            var token = await _service.Authentication.CreateToken();
            return Ok(new { Token = token });
        }
    }
}

