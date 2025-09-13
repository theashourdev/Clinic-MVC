using Cilinc_System.Models;
using Cilinc_System.Repositories.IRepositories;
using ClinicApp.Models;
using ClinicApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Cilinc_System.Repositories
{
    public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
    {
        private readonly ClinicContext _context;

        public AppointmentRepository(ClinicContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.DoctorID == doctorId && a.Date.Date == date.Date)
                .ToListAsync();
        }

        public bool HasAppointmentsForDoctor(int doctorId)
        {
            return _context.Appointments.Any(a => a.DoctorID == doctorId);
        }

    }

}
