using DistributedSystem.Domain.Abstractions;
using DistributedSystem.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystem.Application.Behaviors;

public sealed class TransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;  // SQL-SERVER-STRATEGY-2
    private readonly ApplicationDbContext _applicationDbContext;   // SQL-SERVER-STRATEGY-1 !Break Clean Architecture Rule (Application ref to Persistence)

    public TransactionPipelineBehavior(IUnitOfWork unitOfWork, ApplicationDbContext applocationDbContext)
    {
        _unitOfWork = unitOfWork;
        _applicationDbContext = applocationDbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!IsCommand())
        {
            return await next();
        }

        #region ============== SQL-SERVER-STRATEGY-1 ==============
        //// Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
        //// https://learn.microsoft.com/ef/core/miscellaneous/connection-resiliency
        var strategy = _applicationDbContext.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _applicationDbContext.Database.BeginTransactionAsync();
            {
                var response = await next();
                await _applicationDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return response;
            }
        });
        #endregion ============== SQL-SERVER-STRATEGY-1 ==============

        #region ============== SQL-SERVER-STRATEGY-2 ==============

        //IMPORTANT: passing "TransactionScopeAsyncFlowOption.Enabled" to the TransactionScope constructor. This is necessary to be able to use it with async/await.
        //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        //{
        //    var response = await next();
        //    await _unitOfWork.SaveChangesAsync(cancellationToken);
        //    transaction.Complete();
        //    await _unitOfWork.DisposeAsync();
        //    return response;
        //}

        #endregion ============== SQL-SERVER-STRATEGY-2 ==============
    }

    private bool IsCommand()
        => typeof(TRequest).Name.EndsWith("Command");
}
