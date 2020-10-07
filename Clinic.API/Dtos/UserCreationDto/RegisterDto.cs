using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Clinic.API.Dtos.SystemUsersDto
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        
        public string Status { get; set; }
        
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; } 
         
        public string Country { get; set; }
        
        public string Email { get; set; }
        
        public string AboutMe { get; set; }
        
        public string Address { get; set; }
        
        [JsonIgnore]
        public string Role { get; set; }

    }

}