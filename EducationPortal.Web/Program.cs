using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using EducationPortal.Web.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using EducationPortal.Web.Helpers;
using EducationPortal.Web.Hubs;


var builder = WebApplication.CreateBuilder(args);

// SERVICES
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();


builder.Services.AddDbContext<EducationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();


builder.Services.AddSession();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// --- SEED ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EducationContext>();

    context.Database.EnsureCreated();

    // Eğer admin yoksa bir tane ekle (HASH'LI)
    if (!context.Users.Any(u => u.Email == "admin@portal.com" && u.Role == "Admin"))
    {
        var adminUser = new User
        {
            FullName = "Sistem Yöneticisi",
            Email = "admin@portal.com",
            PasswordHash = PasswordHelper.Hash("123"), // admin şifresi: 123
            Role = "Admin"
        };

        context.Users.Add(adminUser);
        context.SaveChanges();
    }
}
// --- SEED BİTİŞ ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.MapHub<NotificationHub>("/notificationHub");




app.Run();

