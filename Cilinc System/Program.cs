using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Cilinc_System.Repositories;
using Cilinc_System.Repositories.IRepositories;
using Cilinc_System.Services;
using Cilinc_System.Services.IServices;
using ClinicApp.IRepositories.IRepositories;
using ClinicApp.Models;
using ClinicApp.Repositories;
using ClinicApp.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ToastNotification configuration
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 3;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

// Register DbContext
builder.Services.AddDbContext<ClinicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<ISpecialtiesRepository, SpecialtiesRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();

// Register Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<ISpecialtiesService, SpecialtiesService>();
builder.Services.AddScoped<IImageManager, ImageManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
app.UseExceptionHandler("/Home/Error");
app.UseHsts();
//}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseNotyf();

app.UseRouting();

app.UseAuthorization();

app.MapGet("/test-exception", () =>
{
    throw new Exception("Test exception");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
