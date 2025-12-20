using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace fitcensys.Models
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            // Servisleri çağırıyoruz
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. Rolleri Tanımla (Yoksa Oluştur)
            string[] roleNames = { "Admin", "Member" };

            foreach (var roleName in roleNames)
            {
                // Veritabanında bu rol var mı?
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Yoksa oluştur
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Admin Kullanıcısını Bul ve Yetki Ver
            string adminEmail = "b221210023@sakarya.edu.tr";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser != null)
            {
                // Kullanıcı zaten Admin mi? Değilse ekle.
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}