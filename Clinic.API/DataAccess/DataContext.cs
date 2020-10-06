using Clinic.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Clinic.API.DataAccess
{
    public class DataContext : IdentityDbContext<SystemUser>
    {
        public DataContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<SystemUser>  SystemUsers { get; set; }


    }
}