using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Courses;
using MVC_Projekt_Elearning.Helpers;


namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public CourseController(ICourseService courseService,
                                  IWebHostEnvironment env,
                                  ICategoryService categoryService)
                             
        {
            _courseService = courseService;
            _env = env;
            _categoryService = categoryService;
           
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var courses = await _courseService.GetAllPaginateAsync(page, 4);

            var mappedDatas = _courseService.GetMappedDatas(courses);
            int totalPage = await GetPageCountAsync(4);

            Paginate<CourseVM> paginateDatas = new(mappedDatas, totalPage, page);

            return View(paginateDatas);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            int productCount = await _courseService.GetCountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }
    }
}
