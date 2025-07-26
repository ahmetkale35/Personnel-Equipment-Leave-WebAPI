
using System.ComponentModel.DataAnnotations;


namespace Entities.DataTransferObject
{
    public record UserForRegistirationDto
    {
        public string? FirstName { get; init; }

        public string? LastName { get; init; }

        [Required(ErrorMessage = "Kullanıcı adı boş olamaz.")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Şifre boş olamaz.")]
        public string? Password { get; init; }

        public string? Email { get; init; }

        public string? PhoneNumber { get; init; }

        public ICollection<string>? Roles { get; init; }
        
    }
}
