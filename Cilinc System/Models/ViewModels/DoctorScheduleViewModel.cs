namespace Cilinc_System.Models.ViewModels
{
    public class DoctorScheduleViewModel
    {
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsWorking { get; set; }
    }
}
