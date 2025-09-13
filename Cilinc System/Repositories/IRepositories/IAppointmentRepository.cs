using Cilinc_System.Models;
using ClinicApp.IRepositories.IRepositories;

namespace Cilinc_System.Repositories.IRepositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByDoctorAndDateAsync(int doctorId, DateTime date);
        bool HasAppointmentsForDoctor(int doctorId);
    }
}
