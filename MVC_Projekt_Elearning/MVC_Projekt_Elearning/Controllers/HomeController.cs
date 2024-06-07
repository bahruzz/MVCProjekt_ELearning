using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels;
using System.Diagnostics;

namespace MVC_Projekt_Elearning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IInformationService _informationService;

        public HomeController(ISliderService sliderservice,IInformationService information)
        {
            _sliderService = sliderservice;
            _informationService = information;

        }


        public async Task<IActionResult> Index()
        {
            HomeVM model = new()
            {
                Sliders = await _sliderService.GetAllAsync(),
                 Informations = await _informationService.GetAllAsync()

            };
            return View(model);
        }

    }
}
