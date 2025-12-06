using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using EducationPortal.Web.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EducationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CourseRepository>();

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

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EducationContext>();

    // DB yoksa oluştursun, varsa migration'ları uygulasın
    context.Database.EnsureCreated();
    // veya istersen: context.Database.Migrate();

    // Eğer hiç kullanıcı yoksa bir tane Admin ekle
    if (!context.Users.Any())
    {
        context.Users.Add(new User
        {
            FullName = "Sistem Yöneticisi",
            Email = "admin@portal.com",
            PasswordHash = "123",   // GİRİŞTE KULLANACAĞIN ŞİFRE
            Role = "Admin"
        });

        context.SaveChanges();
    }
}


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

app.Run();
