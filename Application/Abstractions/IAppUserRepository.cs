namespace Application;

public interface IAppUserRepository
{
    Task<Guid?> RegisterAppUserAsync(
        string identityId,
        string firstName,
        string lastName,
        string emailAddress);
}
