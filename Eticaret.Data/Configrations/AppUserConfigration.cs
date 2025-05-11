using Eticaret.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eticaret.Data.Configrations
{
    internal class AppUserConfigration : IEntityTypeConfiguration<AppUser>
    {
        //AppUser class'nın veritablosundaki oluşacak olan kolonlarının tiplerini ayarladık
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50); //Max 50 karekter olsun
            builder.Property(x => x.Surname).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Email).IsRequired().HasColumnType("varchar(50)").HasMaxLength(50);
            builder.Property(x => x.Phone).HasColumnType("varchar(15)").HasMaxLength(15);
            builder.Property(x => x.Password).IsRequired().HasColumnType("nvarchar(50)").HasMaxLength(50);
            builder.Property(x => x.UserName) .HasColumnType("nvarchar(50)").HasMaxLength(50);
            builder.HasData(
                new AppUser
                {
                    Id = 1,
                    UserName = "Ahad",
                    Email="aghayev.ahad@gmail.com",
                    IsActive = true,
                    IsAdmin = true,
                    Name="Ahad",
                    Password="123456",
                    Surname="Aghayev"
                }
                );
        }
    }
}
