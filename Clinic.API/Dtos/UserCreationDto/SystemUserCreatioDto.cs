using System;

namespace Clinic.API.Dtos.UserCreationDto
{
    public class SystemUserCreationDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
    }
}