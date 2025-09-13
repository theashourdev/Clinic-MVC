using Cilinc_System.Models.Enums;

namespace Cilinc_System.Models.ViewModels
{
    public class AppointmentIndexViewModel
    {
        public string EncryptedId { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string DoctorName { get; set; } = string.Empty;
        public string PatientName { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }
    }

}
