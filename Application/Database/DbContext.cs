using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Application;

public class AssetDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Asset> Assets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}
