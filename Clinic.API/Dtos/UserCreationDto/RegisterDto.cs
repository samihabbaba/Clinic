using System;
using System.ComponentModel.DataAnnotations;

namespace Clinic.API.Dtos.SystemUsersDto
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8 , MinimumLength = 4 , ErrorMessage = "Please specify password between 4 and 8 characters")]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Country { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}