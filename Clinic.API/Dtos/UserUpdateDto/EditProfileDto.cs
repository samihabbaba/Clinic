using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Clinic.API.Dtos.UserUpdateDto
{
    public class EditProfileDto
    {
        [JsonIgnore]
        public string Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        // public int Phone { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string AboutMe { get; set; }
    }
}