using DistributedSystem.Contract.Abstractions.Message;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Persistence;
using DistributedSystem.Persistence.Outbox;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System.Data;

namespace DistributedSystem.Infrastructure.BackgroundJobs;

// [DisallowConcurrentExecution]: Ngăn chặn các instance khác nhau của cùng một job chạy đồng thời
// Nếu job trước đó vẫn đang chạy khi đến lần trigger tiếp theo sau 100s, job mới sẽ bị hoãn lại (queued) cho đến khi job hiện tại hoàn thành.
[DisallowConcurrentExecution]
public class ProcessOutBoxMessagesJob : IJob
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint; // Rebus

    public ProcessOutBoxMessagesJob(ApplicationDbContext dbContext, IPublishEndpoint publistEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publistEndpoint;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All, // Lấy thông tin kiểu dữ liệu($type)
            });

            if (domainEvent is null)
            {
                continue;
            }

            try
            {
                switch (domainEvent.GetType().Name)
                {
                    case nameof(DomainEvent.ProductCreated):
                        var productCreated = JsonConvert.DeserializeObject<DomainEvent.ProductCreated>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.ProductCreated>(message: productCreated, context.CancellationToken);
                        break;

                    case nameof(DomainEvent.ProductUpdated):
                        var productUpdated = JsonConvert.DeserializeObject<DomainEvent.ProductUpdated>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.ProductUpdated>(message: productUpdated, context.CancellationToken);
                        break;

                    case nameof(DomainEvent.ProductDeleted):
                        var productDeleted = JsonConvert.DeserializeObject<DomainEvent.ProductDeleted>(
                                    outboxMessage.Content,
                                    new JsonSerializerSettings
                                    {
                                        TypeNameHandling = TypeNameHandling.All
                                    });
                        await _publishEndpoint.Publish<DomainEvent.ProductDeleted>(message: productDeleted, context.CancellationToken);
                        break;
                    default:
                        break;
                }

                outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                outboxMessage.Error = ex.Message;
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}
