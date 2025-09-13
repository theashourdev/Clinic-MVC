using Cilinc_System.Models;

namespace Cilinc_System.Services.IServices
{
    public interface ISpecialtiesService
    {
        IEnumerable<Specialties> GetAll();
        Specialties? GetById(int id);
        void Create(Specialties specialties);
        void Update(Specialties specialties);
        void Delete(int id);
    }
}