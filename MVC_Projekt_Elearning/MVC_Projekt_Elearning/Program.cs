using Microsoft.EntityFrameworkCore;
using MVC_Projekt_Elearning.Data;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.Services;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IInformationService, InformationService>();






var app = builder.Build();

app.UseStaticFiles();

app.UseHttpLogging();

app.UseRouting();

app.MapControllerRoute(
     name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





app.Run();
