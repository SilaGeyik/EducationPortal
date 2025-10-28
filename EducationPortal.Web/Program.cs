using EducationPortal.Web.Data;
using EducationPortal.Web.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Servisler ---
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<EducationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Oturum (Session) servisini ekle
builder.Services.AddSession();

var app = builder.Build();

// --- Seed işlemi ---
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
            Password = "12345",
            Role = "Admin"
        });
        context.SaveChanges();
    }
}

// --- Middleware sırası ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// ✅ Statik dosyaları oku (wwwroot altındaki templates vb.)
app.UseStaticFiles();

app.UseRouting();

// ✅ Session middleware (Authorization'dan ÖNCE olacak!)
app.UseSession();

app.UseAuthorization();

// ✅ Varsayılan yönlendirme
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
