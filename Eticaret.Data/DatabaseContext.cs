using Eticaret.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Eticaret.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> db) : base(db)
        {

        }
        public DbSet<Adress> Addresses { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer(@"Server=ahad;Database=EticaretDb;Trusted_Connection=True; TrustServerCertificate=True;");
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new AppUserConfigration());
            //modelBuilder.ApplyConfiguration(new BrandConfigration());

            //Tüm konfigürasyonları otomatik olarak uygulanmasının yapılması
            //çalışan dll'in içinden bul
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
