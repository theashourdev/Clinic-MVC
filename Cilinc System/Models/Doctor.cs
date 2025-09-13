using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cilinc_System.Models
{
    public class Doctor
    {
        [Key]
        public int DoctorID { get; set; }

        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? ImagePath { get; set; }

        public ICollection<DoctorSchedule>? Schedules { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }

        public int SpecialtyID { get; set; }

        [ForeignKey("SpecialtyID")]
        public Specialties Specialty { get; set; }


    }
}
