using Microsoft.EntityFrameworkCore;
using MVC_Projekt_Elearning.Models;

namespace MVC_Projekt_Elearning.Data
{
    public class AppDbContext: DbContext

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Slider>().HasQueryFilter(m => !m.SoftDeleted);
        }


    }
}
