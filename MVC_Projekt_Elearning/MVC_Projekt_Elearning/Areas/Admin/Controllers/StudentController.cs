using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Services.Interfaces;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _env;
        public StudentController(IStudentService studentService,
                                   IWebHostEnvironment env)
        {
            _studentService = studentService;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _studentService.GetAllAsync());
        }
    }
}
