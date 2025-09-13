using Cilinc_System.Models;

namespace Cilinc_System.Services.IServices
{

    public interface IDoctorService
    {
        IEnumerable<Doctor> GetAll();
        Doctor? GetById(int id);
        void Create(Doctor doctor);
        void UpdateWithSchedules(Doctor doctor);
        void Delete(int id);
    }
}
