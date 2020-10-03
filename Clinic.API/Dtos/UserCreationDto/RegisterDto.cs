using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Clinic.API.Dtos.SystemUsersDto
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }     
        public string Country { get; set; }
        public string Email { get; set; }
        
        // [Phone]
        // public int PhoneNumber { get; set; }

        [JsonIgnore]
        public string Role { get; set; }

    }

}