using System.ComponentModel.DataAnnotations;

namespace Cilinc_System.Models
{
    public class Clinic
    {
        [Key]
        public int ClinicID { get; set; }

        [Required, MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }
        public ICollection<Doctor>? Doctors { get; set; }

    }
}
