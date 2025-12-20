using fitcensys.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using fitcensys.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. VERİTABANI BAĞLANTISI
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. IDENTITY AYARLARI (TEK VE DOĞRU OLAN)
// Scaffolding'in eklediği 'AddDefaultIdentity' satırını sildik.
// Rol yönetimi (IdentityRole) olduğu için bunu kullanıyoruz.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Şifre Zorluk Ayarları (Geliştirme aşaması için basit bıraktık)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 3;

    // Email Tekilliği
    options.User.RequireUniqueEmail = true;

    // Email onayı zorunluluğunu kapattık (Demo proje için kolaylık olsun)
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// EmailSender servisini sisteme tanıtıyoruz
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// 3. SERVİSLERİN EKLENMESİ
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // <--- BUNU EKLEMEK ŞART! (Identity sayfaları için)

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    // Yazdığımız Seeder metodunu çağırıyoruz
    // Hata olursa program patlamasın diye try-catch koyabilirsin ama şimdilik gerek yok, görsek iyi olur.
    await fitcensys.Models.RoleSeeder.SeedRolesAndAdminAsync(services);
}

// 4. HTTP REQUEST PIPELINE
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Önce Kimlik Doğrulama (Kimsin?), Sonra Yetkilendirme (Neye iznin var?)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // <--- BUNU EKLEMEK ŞART! (Login/Register linklerinin çalışması için)

app.Run();