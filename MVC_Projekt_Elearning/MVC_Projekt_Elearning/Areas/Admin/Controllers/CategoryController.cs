using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Categories;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public CategoryController(ICategoryService categoryService,
                                  IWebHostEnvironment env)
        {
            _categoryService = categoryService;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAlWithProductCountAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existCategory = await _categoryService.ExistAsync(category.Name);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }
            if (!category.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input accept only image format");
                return View();
            }
            if (!category.Image.CheckFileSize(500))
            {
                ModelState.AddModelError("Image", "Image size must be 500 kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + category.Image.FileName;
            string path = _env.GenerateFilePath("img", fileName);
            await category.Image.SaveFileToLocalAsync(path);
            await _categoryService.CreateAsync(new Category { Name = category.Name,  Image = fileName });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var about = await _categoryService.GetByIdAsync((int)id);
            if (about is null) return NotFound();
            string path = _env.GenerateFilePath("img", about.Image);
            path.DeleteFileFromLocal();
            await _categoryService.DeleteAsync(about);
            return RedirectToAction(nameof(Index));

        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Category category = await _categoryService.GetByIdAsync((int)id);


            return View(category);
        }
    }
}
