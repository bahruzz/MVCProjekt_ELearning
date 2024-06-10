using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Projekt_Elearning.Models;

namespace MVC_Projekt_Elearning.Services.Interfaces
{
    public interface IInstructorService
    {
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task CreateAsync(Instructor instructor);
        Task DeleteAsync(Instructor instructor);
        Task EditAsync();
        Task<SelectList> GetAllSelectedAsync();
    }
}
