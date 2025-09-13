using Cilinc_System.Models;
using Cilinc_System.Repositories.IRepositories;
using Cilinc_System.Services.IServices;

namespace Cilinc_System.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public IEnumerable<Doctor> GetAll()
        {
            return _doctorRepository.Get(
                includes: [d => d.Specialty]
            );
        }


        public Doctor? GetById(int id)
        {
            return _doctorRepository.GetOne(d => d.DoctorID == id, [d => d.Schedules!, d => d.Specialty]);
        }

        public void Create(Doctor doctor)
        {
            _doctorRepository.Create(doctor);
            _doctorRepository.Commit();
        }


        public void UpdateWithSchedules(Doctor doctor)
        {
            _doctorRepository.UpdateWithSchedules(doctor);
        }


        public void Delete(int id)
        {
            var doctor = _doctorRepository.GetOne(d => d.DoctorID == id);
            if (doctor != null)
            {
                _doctorRepository.Delete(doctor);
                _doctorRepository.Commit();
            }
        }
    }
}
