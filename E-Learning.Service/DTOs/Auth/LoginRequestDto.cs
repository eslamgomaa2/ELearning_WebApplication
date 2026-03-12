using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace E_Learning.Service.DTOs.Auth
{

    public class LoginRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
