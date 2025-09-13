using Cilinc_System.Models;
using Cilinc_System.Repositories.IRepositories;
using ClinicApp.Models;
using ClinicApp.Repositories;

namespace Cilinc_System.Repositories
{
    public class SpecialtiesRepository : Repository<Specialties>, ISpecialtiesRepository
    {
        public SpecialtiesRepository(ClinicContext appDbContext) : base(appDbContext)
        {
        }
    }
}
