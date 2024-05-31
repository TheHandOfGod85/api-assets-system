using Application;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppUserRepository(AssetDbContext _dbContext) : IAppUserRepository
{
    public async Task<Guid?> RegisterAppUserAsync(
        string identityId,
        string firstName,
        string lastName,
        string emailAddress)
    {
        var exists = await _dbContext.AppUsers.AnyAsync(appUser => appUser.IdentityId == identityId);
        if (exists) return null;
        var appUser = new AppUser(identityId, firstName, lastName, emailAddress);
        _dbContext.Add(appUser);
        await _dbContext.SaveChangesAsync();
        return appUser.Id;
    }
}
