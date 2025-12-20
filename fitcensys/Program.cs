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


// Program.cs içinde app.Run()'dan hemen önce şu bloğu ekle:
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // 1. Rolleri oluştur (Yoksa)
        string[] roleNames = { "Admin", "Member" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        // 2. Sabit Admin Kullanıcısını Oluştur
        var adminEmail = "admin@fitcensys.com"; // Hocaya bu maili ver
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Sistem",
                LastName = "Admin",
                EmailConfirmed = true,
                Height = 180, // Default değerler
                Weight = 80,
                BirthDate = new DateTime(1990, 1, 1)
            };

            var createAdmin = await userManager.CreateAsync(newAdmin, "Admin123!"); // Sabit Şifre
            if (createAdmin.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
            }
        }
    }
    catch (Exception ex)
    {
        // Hata loglanabilir
    }
}

app.Run();
app.Run();