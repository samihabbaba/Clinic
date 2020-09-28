using System;
using System.ComponentModel.DataAnnotations;

namespace Clinic.API.Dtos.SystemUsersDto
{
    public class RegisterDto
    {
       
        public string UserName { get; set; }

        public string Password { get; set; }


        public string Gender { get; set; }

    
        // public DateTime DateOfBirth { get; set; }

       
        public string Country { get; set; }

        // [Phone]
        // public string PhoneNumber { get; set; }
        public string Role { get; set; }

    }

}