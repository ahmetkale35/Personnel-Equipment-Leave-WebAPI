using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Services
{
    public class AuthenticationManager : IAuthenticationService
    {
        private User? _user;


        // Servislerin bağımlılıklarını alır
        
        public readonly IMapper _mapper;
        public readonly UserManager<User> _userManager; 
        public readonly IConfiguration _configuration;
        private ILoggerService _logger;

        public AuthenticationManager(IMapper mapper, UserManager<User> userManager, IConfiguration configuration, ILoggerService logger)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        // Asenkron olarak kullanıcı kaydı yapar
        public async Task<IdentityResult> RegisterUser(UserForRegistirationDto userForRegistirationDto)
        {
            // Kullanıcıyı DTO'dan User modeline dönüştür
            var user = _mapper.Map<User>(userForRegistirationDto);

            //  asenkron olarak kullanıcıyı oluştur
            var result = await _userManager.
                CreateAsync(user, userForRegistirationDto.Password);


            // Eğer kullanıcı kaydı başarılı ise, kullanıcının rollerini ekle   
            if (result.Succeeded)
                await _userManager.AddToRolesAsync(user, userForRegistirationDto.Roles ?? new List<string>());
            return result;
        }

        public async Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto)
        {
            _user = await _userManager.FindByNameAsync(userForAuthDto.UserName);
            var result = (_user != null && await _userManager.CheckPasswordAsync(_user, userForAuthDto.Password));
            if (!result)
            {
                _logger.LogWarning($"{nameof(ValidateUser)} : Authentication failed. Wrong username of password.");
            }
            return result;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigingCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private SigningCredentials GetSigingCredentials()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager
                .GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials,
            List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var tokenOptions = new JwtSecurityToken(
                    issuer: jwtSettings["validIssuer"],
                    audience: jwtSettings["validAudience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                    signingCredentials: signinCredentials);

            return tokenOptions;
        }
    }
}
