﻿using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Abouts;
using MVC_Projekt_Elearning.ViewModels.Informations;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        private readonly IWebHostEnvironment _env;

        public AboutController(IAboutService aboutService, IWebHostEnvironment env)
        {

            _aboutService = aboutService;
            _env = env;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _aboutService.GetAllAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AboutCreateVM about)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existAbout = await _aboutService.ExistAsync(about.Title);
            if (existAbout)
            {
                ModelState.AddModelError("Title", "This title already exist");
                return View();
            }
            if (!about.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input accept only image format");
                return View();
            }
            if (!about.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be 200 kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + about.Image.FileName;
            string path = _env.GenerateFilePath("img", fileName);

            await about.Image.SaveFileToLocalAsync(path);
            await _aboutService.CreateAsync(new About { Title = about.Title, Description = about.Description, Image = fileName });

            return RedirectToAction(nameof(Index));


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var about = await _aboutService.GetByIdAsync((int)id);
            if (about is null) return NotFound();
            string path = _env.GenerateFilePath("img", about.Image);
            path.DeleteFileFromLocal();
            await _aboutService.DeleteAsync(about);
            return RedirectToAction(nameof(Index));

        }
    }
}