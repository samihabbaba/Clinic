using System;

namespace Clinic.API.Dtos.UserListDto
{
    public class ListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }
    
    }
}