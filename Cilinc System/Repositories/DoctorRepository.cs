using Cilinc_System.Models;
using Cilinc_System.Repositories.IRepositories;
using ClinicApp.Models;
using ClinicApp.Repositories;


namespace Cilinc_System.Repositories
{
    public class DoctorRepository : Repository<Doctor>, IDoctorRepository
    {
        private readonly ClinicContext _context;
        public DoctorRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateWithSchedules(Doctor doctor)
        {
            var existingDoctor = _context.Doctors
                .FirstOrDefault(d => d.DoctorID == doctor.DoctorID);

            if (existingDoctor == null) return;

            // Update doctor basic info
            existingDoctor.Name = doctor.Name;
            existingDoctor.Phone = doctor.Phone;
            existingDoctor.SpecialtyID = doctor.SpecialtyID;
            existingDoctor.ImagePath = doctor.ImagePath;

            // Remove old schedules safely
            var oldSchedules = _context.DoctorSchedules
                .Where(s => s.DoctorID == doctor.DoctorID)
                .ToList();

            _context.DoctorSchedules.RemoveRange(oldSchedules);

            // Add new schedules from a separate copy
            var newSchedules = doctor.Schedules!
                .Select(s => new DoctorSchedule
                {
                    DoctorID = doctor.DoctorID,
                    DayOfWeek = s.DayOfWeek,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    IsWorking = s.IsWorking
                })
                .ToList();

            _context.DoctorSchedules.AddRange(newSchedules);

            _context.SaveChanges();
        }


    }
}
