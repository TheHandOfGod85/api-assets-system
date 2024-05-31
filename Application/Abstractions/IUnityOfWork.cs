namespace Application;

public interface IUnitOfWork
{
    IAssetRepository Assets { get; }
    IAppUserRepository AppUsers { get; }
    IDepartmentRepository Departments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task StartTransactionAsync(CancellationToken cancellationToken);
    Task SubmitTransactionAsync(CancellationToken cancellationToken);
    Task RevertTransactionAsync(CancellationToken cancellationToken);
}
