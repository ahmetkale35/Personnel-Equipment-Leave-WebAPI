using Microsoft.AspNetCore.Identity;

using Entities.DataTransferObject;

namespace Services.Contracts
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistirationDto userForRegistirationDto);
        Task<bool> ValidateUser(UserForAuthenticationDto userForAuthDto);

        Task<string> CreateToken();

    }
}
