using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ClinicApp.Models.ViewModels
{
    public class AppointmentViewModel
    {
        [Required] public string? PatientName { get; set; }
        [Required] public DateTime BirthDate { get; set; }
        [Required] public string? Phone { get; set; }

        // Selections
        [Required] public int SpecialtyId { get; set; }
        [Required] public int DoctorId { get; set; }
        [Required] public DateTime Date { get; set; }
        [Required] public TimeSpan Time { get; set; }

        // Dropdowns
        public List<SelectListItem>? Specialties { get; set; }
        public List<SelectListItem>? Doctors { get; set; }
        public List<SelectListItem>? AvailableSlots { get; set; }
    }
}
