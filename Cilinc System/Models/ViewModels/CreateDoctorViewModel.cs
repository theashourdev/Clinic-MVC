using System.ComponentModel.DataAnnotations;

namespace Cilinc_System.Models.ViewModels
{
    public class CreateDoctorViewModel
    {


        [Required]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Sepcialty is required.")]
        public int SpecialtyID { get; set; }

        public string? Phone { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Schedules is required.")]
        public List<DoctorScheduleViewModel> Schedules { get; set; }



    }
}
