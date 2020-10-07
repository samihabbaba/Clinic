using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Clinic.API.Models
{
    public class Appointments
    {
        [Key]
        public int Id { get; set; } 
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public virtual SystemUser Doctor { get; set; }
         [ForeignKey("PatientId")]
        public virtual SystemUser Patient { get; set; }

    }
}