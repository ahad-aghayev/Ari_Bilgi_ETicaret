using Eticaret.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eticaret.Data.Configrations
{
    public class SliderConfigration :IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
    {
        builder.Property(x => x.Title) .HasMaxLength(250);
        builder.Property(x => x.Description).HasMaxLength(750);
        builder.Property(x => x.Image).HasMaxLength(100);
        builder.Property(x => x.Link).HasMaxLength(100);
    }
}
}
