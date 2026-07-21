using GavGavich.Application.Abstractions;
using GavGavich.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GavGavich.Infrastructure.Repositories;

public sealed class EfRepository<T>(AppDbContext context) : IRepository<T> where T : class
{
    private readonly DbSet<T> _set = context.Set<T>();

    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _set.FindAsync([id], cancellationToken).AsTask();

    public async Task<IReadOnlyList<T>> ListAsync(CancellationToken cancellationToken = default) =>
        await _set.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _set.AddAsync(entity, cancellationToken);

    public IQueryable<T> Query() => _set;
}
