using Cilinc_System.Models;
using ClinicApp.IRepositories.IRepositories;


namespace Cilinc_System.Repositories.IRepositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        void UpdateWithSchedules(Doctor doctor);
    }
}
