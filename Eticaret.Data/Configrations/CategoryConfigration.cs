using Eticaret.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eticaret.Data.Configrations
{
    public class CategoryConfigration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Image).HasMaxLength(50);

            //ilk örnek Kategorini oluştur 
            builder.HasData(
                new Category
                {
                    Id = 1,
                    Name = "Elektronik",
                    IsActive = true,
                    IsTopMenu = true,
                    ParrentId = 0,
                    OrderNo = 1,
                },
                // 2. örnek Kategorini oluştur
                new Category
                {
                    Id = 2,
                    Name = "Bilgisayar",
                    IsActive = true,
                    IsTopMenu = true,
                    ParrentId = 0,
                    OrderNo = 2,
                }
                );
        }
    }
}
