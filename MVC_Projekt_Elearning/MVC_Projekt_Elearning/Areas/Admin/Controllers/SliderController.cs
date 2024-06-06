using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Sliders;
using System.Reflection.Metadata;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly ISliderService _sliderService;
        private readonly IWebHostEnvironment _env;

        public SliderController(ISliderService sliderService,IWebHostEnvironment env )
        {
           
            _sliderService = sliderService;
            _env = env;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _sliderService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM slider) 
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existBlog = await _sliderService.ExistAsync(slider.Title);
            if (existBlog)
            {
                ModelState.AddModelError("Title", "This title already exist");
                return View();
            }
            if (!slider.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input accept only image format");
                return View();
            }
            if (!slider.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be 200 kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + slider.Image.FileName;
            string path = _env.GenerateFilePath("img", fileName);

            await slider.Image.SaveFileToLocalAsync(path);
            await _sliderService.CreateAsync(new Slider { Title = slider.Title, Description = slider.Description, Image = fileName });

            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var slider = await _sliderService.GetByIdAsync((int)id);
            if (slider is null) return NotFound();
            string path = _env.GenerateFilePath("img", slider.Image);
            path.DeleteFileFromLocal();
            await _sliderService.DeleteAsync(slider);
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Slider slider = await _sliderService.GetByIdAsync((int)id);


            return View(slider);
        }

    }
}
