
using System.ComponentModel.DataAnnotations;

namespace E_Learning.Service.DTOs.Auth
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
