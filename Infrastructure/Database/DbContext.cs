using System.Reflection;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AssetDbContext : DbContext
{
    public AssetDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

}
