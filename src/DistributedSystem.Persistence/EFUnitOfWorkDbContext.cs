using DistributedSystem.Domain.Abstractions;
using DistributedSystem.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystem.Persistence;

public class EFUnitOfWorkDbContext<TContext> : IUnitOfWorkDbContext<TContext>
    where TContext : DbContext
{
    private readonly ApplicationDbContext _dbContext;

    public EFUnitOfWorkDbContext(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {      
        await _dbContext.DisposeAsync();
    }

    public async Task SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync();
    }
}
