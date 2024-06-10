using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Courses;
using MVC_Projekt_Elearning.Helpers;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services;


namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IInstructorService _instructorService;
        public CourseController(ICourseService courseService,
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


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            ViewBag.instructor = await _instructorService.GetAllSelectedAsync();
            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            ViewBag.instructor = await _instructorService.GetAllSelectedAsync();
            if (!ModelState.IsValid)
            {
                return View();

            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Images", "Image size must be max 500 KB");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be only image");

                    return View();
                }
            }
            List<CourseImage> images = new();
            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                string path = _env.GenerateFilePath("img", fileName);
                await item.SaveFileToLocalAsync(path);
                images.Add(new CourseImage { Name = fileName });
            }

            images.FirstOrDefault().IsMain = true;
            Course course = new()
            {
                Name = request.Name,
                Duration = request.Duration,
                Rating = request.Rating,
                InstructorId = request.InstructorId,
                CategoryId = request.CategoryId,
                Price = decimal.Parse(request.Price.Replace(".", ",")),
                CoursesImages = images

            };

            await _courseService.CreateAsync(course);


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var existProduct = await _courseService.GetByIdWithCoursesImagesAsync((int)id);
            if (existProduct is null) return NotFound();

            foreach (var item in existProduct.CoursesImages)
            {
                string path = _env.GenerateFilePath("img", item.Name);

                path.DeleteFileFromLocal();
            }
            await _courseService.DeleteAsync(existProduct);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Course course = await _courseService.GetByIdWithCoursesImagesAsync((int)id);


            return View(course);
        }


    }


}
