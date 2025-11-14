using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using EducationPortal.Web.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Servisler ---
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EducationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddSession();

// 👉 Eksikti! MUTLAKA eklenmeli
builder.Services.AddAuthorization();

// 👉 Authentication servisi doğru
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });

var app = builder.Build();

// --- Seed admin ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EducationContext>();
    context.Database.Migrate();

    if (!context.Users.Any(u => u.Role == "Admin"))
    {
        context.Users.Add(new User
        {
            FullName = "Admin Kullanıcı",
            Email = "admin@portal.com",
            Password = "123",
            Role = "Admin"
        });
        context.SaveChanges();
    }
}

// --- Middleware ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

// 👉 Sırası doğru! Authentication → Authorization
app.UseAuthentication();
app.UseAuthorization();

// 👉 Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
