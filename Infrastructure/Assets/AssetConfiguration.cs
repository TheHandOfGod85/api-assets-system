using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;

public class AssetConfiguration : IEntityTypeConfiguration<Asset>
{
    public void Configure(EntityTypeBuilder<Asset> builder)
    {
        builder.HasIndex(asset => asset.SerialNumber).IsUnique();
        builder.HasOne(x => x.Department)
                .WithOne(x => x.Asset)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
    }
}
