using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Instructors;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("admin")]
    public class InstructorController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IInstructorService _instructorService;
        private readonly IWebHostEnvironment _env;

        public InstructorController(ICourseService courseService,
                                 IWebHostEnvironment env,
                                 ICategoryService categoryService,
                                 IInstructorService instructorService)

        {
            _courseService = courseService;
            _env = env;
            _categoryService = categoryService;
            _instructorService = instructorService;

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var instructor = await _instructorService.GetAllAsync();


            List<InstructorVM> instructors = instructor.Select(m => new InstructorVM { Id = m.Id, FullName = m.FullName, Email = m.Email, Image = m.Image, Designation = m.Designation }).ToList();

            return View(instructors);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existInstructor = await _instructorService.ExistEmailAsync(request.Email);
            if (existInstructor)
            {
                ModelState.AddModelError("Name", "This Instructor already exist");
                return View();
            }


            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }
            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB ");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;

            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await request.Image.SaveFileToLocalAsync(path);



            await _instructorService.CreateAsync(new Instructor { FullName = request.FullName, Image = fileName, Email = request.Email, Designation = request.Designation });
            return RedirectToAction(nameof(Index));

        }
    }
}
