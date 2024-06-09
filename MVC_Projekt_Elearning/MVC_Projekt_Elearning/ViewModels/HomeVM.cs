﻿using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Abouts;
using MVC_Projekt_Elearning.ViewModels.Categories;
using MVC_Projekt_Elearning.ViewModels.Informations;
using MVC_Projekt_Elearning.ViewModels.Sliders;

namespace MVC_Projekt_Elearning.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<SliderVM> Sliders { get; set; }
        public IEnumerable<InformationVM> Informations { get; set; }
        public About Abouts { get; set; }
        public CategoryCourseVM CategoryFirst { get; set; }
        public CategoryCourseVM CategoryLast { get; set; }
        public IEnumerable<CategoryCourseVM> Categories { get; set; }
    }
}
