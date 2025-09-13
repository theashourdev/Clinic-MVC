using System.ComponentModel.DataAnnotations;

namespace Cilinc_System.Models
{
    public class Patient
    {
        [Key]
        public int PatientID { get; set; }

        [Required, MaxLength(100)]
        public string? Name { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        // Navigation
        public ICollection<Appointment>? Appointments { get; set; }
    }
}
