using Cilinc_System.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Cilinc_System.Models
{
    public class Specialties : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; } = null!;

        [MaxLength(255)]
        public string? ImagePath { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new HashSet<Doctor>();

        public Specialties()
        {

        }

        public Specialties(CreateSpecialtiesViewModel model)
        {
            Name = model.Name;
            Description = model.Description;
            ImagePath = model.ImagePath;
        }
    }
}
