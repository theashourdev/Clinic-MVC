using System.ComponentModel.DataAnnotations;

namespace Cilinc_System.Models.ViewModels
{
    public class CreateSpecialtiesViewModel
    {
        [MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(500)]
        public string Description { get; set; } = default!;
        public IFormFile? ImageFile { get; set; }

        public string? ImagePath { get; set; }
    }
}
