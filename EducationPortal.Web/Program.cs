using EducationPortal.Web.Data;
using EducationPortal.Web.Hubs;
using EducationPortal.Web.Models;
using EducationPortal.Web.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ==================== DB CONTEXT ====================
builder.Services.AddDbContext<EducationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==================== IDENTITY ======================
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.User.RequireUniqueEmail = true;

    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<EducationContext>()
.AddDefaultTokenProviders();

// Cookie ayarları
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});

// ==================== SIGNALR =======================
builder.Services.AddSignalR();

// ==================== REPOSITORY / DİĞER SERVİSLER ==
builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddSession();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// ==================== MIDDLEWARE =====================
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

// ==================== MVC ROUTE =====================
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// ==================== SIGNALR HUB ROUTE =============
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
