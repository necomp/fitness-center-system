namespace fitcensys.Models
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    namespace fitcensys.Models
    {
        // Identity entegrasyonu için IdentityDbContext'ten miras alıyoruz
        public class AppDbContext : IdentityDbContext<ApplicationUser>
        {
            public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
            {
            }
                                                                                    
            public DbSet<Gym> Gyms { get; set; }                                    // Master entity table
            public DbSet<GymWorkingHour> GymWorkingHours { get; set; }
            public DbSet<ServiceDefinition> ServiceDefinitions { get; set; }        // Master entity table
            public DbSet<GymService> GymServices { get; set; }                      // Join entity (ara tablo) Gym+ServiceDef
            public DbSet<Trainer> Trainers { get; set; }                            // Master entity table
            public DbSet<TrainerAvailability> TrainerAvailabilities { get; set; }   // One to many
            public DbSet<TrainerService> TrainerServices { get; set; }              // Join entity (ara tablo) Trainer+ServiceDef
            public DbSet<Appointment> Appointments { get; set; }                    // One to many

            protected override void OnModelCreating(ModelBuilder builder)
            {
                base.OnModelCreating(builder); // Identity tabloları için şart.

                // ilişkiler //////////////////////////////////////

                // TrainerService (Çoka-Çok - Composite Key)
                builder.Entity<TrainerService>()
                    .HasKey(ts => new { ts.TrainerID, ts.ServiceDefinitionID });

                builder.Entity<TrainerService>()
                    .HasOne(ts => ts.Trainer)
                    .WithMany(t => t.TrainerServices)
                    .HasForeignKey(ts => ts.TrainerID);

                builder.Entity<TrainerService>()
                    .HasOne(ts => ts.ServiceDefinition)
                    .WithMany(s => s.TrainerServices)
                    .HasForeignKey(ts => ts.ServiceDefinitionID);

                // Silme Davranışları (Restrict: Yanlışlıkla veri kaybını önler)
                builder.Entity<Appointment>()
                    .HasOne(a => a.GymService)
                    .WithMany(gs => gs.Appointments)
                    .HasForeignKey(a => a.GymServiceID)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.Entity<Appointment>()
                    .HasOne(a => a.Trainer)
                    .WithMany(t => t.Appointments)
                    .HasForeignKey(a => a.TrainerID)
                    .OnDelete(DeleteBehavior.Restrict);

                // Hassasiyet Ayarları
                builder.Entity<GymService>().Property(p => p.Price).HasColumnType("decimal(18,2)");
                builder.Entity<Appointment>().Property(p => p.PriceSnapshot).HasColumnType("decimal(18,2)");
            }
        }
    }
}
