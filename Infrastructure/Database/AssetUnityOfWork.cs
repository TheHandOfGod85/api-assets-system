﻿using Application;

namespace Infrastructure;

public class AssetUnitOfWork : IUnitOfWork
{
    private readonly AssetDbContext _context;
    public AssetUnitOfWork(
        AssetDbContext context,
        IAssetRepository assets,
        IDepartmentRepository departments)
    {
        Assets = assets;
        Departments = departments;
        _context = context;
    }

    public IAssetRepository Assets { get; }
    public IDepartmentRepository Departments { get; }

    public async Task RevertTransactionAsync(CancellationToken cancellationToken)
    {
        await _context.Database.RollbackTransactionAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task StartTransactionAsync(CancellationToken cancellationToken)
    {
        await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task SubmitTransactionAsync(CancellationToken cancellationToken)
    {
        await _context.Database.CommitTransactionAsync(cancellationToken);
    }
}
