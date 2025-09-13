using Cilinc_System.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cilinc_System.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentID { get; set; }

        // Foreign Keys
        public int PatientID { get; set; }
        //public string? PatientName { get; set; }

        [ForeignKey("PatientID")]
        public Patient? Patient { get; set; }

        public int DoctorID { get; set; }
        [ForeignKey("DoctorID")]
        public Doctor? Doctor { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;
        public int VisitLength { get; set; } = 30;
    }
}
