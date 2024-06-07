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
        private readonly IAboutService _aboutService;

        public HomeController(ISliderService sliderservice,IInformationService informationService, IAboutService aboutService)
        {
            _sliderService = sliderservice;
            _informationService = informationService;
            _aboutService = aboutService;
        }


        public async Task<IActionResult> Index()
        {
            HomeVM model = new()
            {
                 Sliders = await _sliderService.GetAllAsync(),
                 Informations = await _informationService.GetAllAsync(),
                Abouts = await _aboutService.GetAboutAsync(),

            };
            return View(model);
        }

    }
}
