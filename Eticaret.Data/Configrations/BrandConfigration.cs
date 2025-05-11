    using Eticaret.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eticaret.Data.Configrations
{
    public class BrandConfigration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(x => x.Name).IsRequired(). HasMaxLength(50); //Max 50 karekter olsun
            builder.Property(x => x.Logo). HasMaxLength(50);
        }
    }
}
