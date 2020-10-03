using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Clinic.API.Models
{
    public class SystemUser:IdentityUser
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    
    }
}