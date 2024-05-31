using System.Reflection;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AssetDbContext : IdentityDbContext<IdentityUser>
{
    public AssetDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
