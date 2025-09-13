using Cilinc_System.Models.ViewModels;
using Cilinc_System.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Cilinc_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDoctorService _doctorService;
        private readonly ISpecialtiesService _specialtiesService;

        public HomeController(IDoctorService doctorService, ISpecialtiesService specialtiesService)
        {
            _doctorService = doctorService;
            _specialtiesService = specialtiesService;
        }

        public IActionResult Index()
        {
            var viewModel = new HomeViewModel
            {
                Doctors = _doctorService.GetAll().ToList(),
                Specialties = _specialtiesService.GetAll().ToList()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}
