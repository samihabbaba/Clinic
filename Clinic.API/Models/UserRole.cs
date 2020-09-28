
using Microsoft.AspNetCore.Identity;

namespace Clinic.API.Models
{
    public class UserRole:IdentityUserRole<string>
    {
        public virtual SystemUser SystemUser { get; set; }
        public virtual Role Role { get; set; }
    }
}