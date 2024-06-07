using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Informations;
using MVC_Projekt_Elearning.ViewModels.Sliders;

namespace MVC_Projekt_Elearning.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<SliderVM> Sliders { get; set; }
        public IEnumerable<InformationVM> Informations { get; set; }
    }
}
