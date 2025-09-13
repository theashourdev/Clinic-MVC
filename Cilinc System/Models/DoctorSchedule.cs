using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cilinc_System.Models
{
    public class DoctorSchedule
    {
        [Key]
        public int ScheduleID { get; set; }

        // Foreign Key
        public int DoctorID { get; set; }

        [ForeignKey("DoctorID")]
        public Doctor? Doctor { get; set; }

        [Required]
        public DayOfWeek DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsWorking { get; set; }
    }
}
