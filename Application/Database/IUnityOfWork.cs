namespace Application;

public interface IUnityOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
