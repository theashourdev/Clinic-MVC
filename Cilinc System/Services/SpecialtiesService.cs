using Cilinc_System.Models;
using Cilinc_System.Repositories.IRepositories;
using Cilinc_System.Services.IServices;

namespace Cilinc_System.Services
{
    public class SpecialtiesService : ISpecialtiesService
    {
        private readonly ISpecialtiesRepository _specialtiesRepository;

        public SpecialtiesService(ISpecialtiesRepository specialtiesRepository)
        {
            _specialtiesRepository = specialtiesRepository;
        }

        public IEnumerable<Specialties> GetAll()
        {
            return _specialtiesRepository.Get();
        }

        public Specialties? GetById(int id)
        {
            return _specialtiesRepository.GetOne(d => d.Id == id);
        }

        public void Create(Specialties specialties)
        {
            _specialtiesRepository.Create(specialties);
            _specialtiesRepository.Commit();
        }

        public void Update(Specialties specialties)
        {
            _specialtiesRepository.Edit(specialties);
            _specialtiesRepository.Commit();
        }

        public void Delete(int id)
        {
            var specialties = _specialtiesRepository.GetOne(d => d.Id == id);
            if (specialties != null)
            {
                _specialtiesRepository.Delete(specialties);
                _specialtiesRepository.Commit();
            }
        }
    }
}
