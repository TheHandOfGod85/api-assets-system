using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.HasKey(dpt => dpt.Name);
        builder
            .HasMany(dpt => dpt.Assets)
            .WithOne(asset => asset.Department)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
