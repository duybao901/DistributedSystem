using DistributedSystem.Persistence.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace DistributedSystem.Persistence.Interceptors;

public sealed class ConvertDomainEventsToOutboxMessageInterceptor
    : SaveChangesInterceptor
{
    // Call before Saving    
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
       DbContextEventData eventData,
       InterceptionResult<int> result,
       CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        // Lấy các AggregateRoot trong DbContext có chứa Domain Events
        var outboxMessages = dbContext.ChangeTracker
            .Entries<Domain.Abstractions.Aggregates.AggregateRoot<Guid>>()
            .Select(x => x.Entity)
            .SelectMany(aggregateRoot =>
            {
                // Lấy danh sách các Domain Events từ AggregateRoot
                var domainEvents = aggregateRoot.GetDomainEvents();

                // Xóa các Domain Events sau khi lấy ra để tránh gửi đi nhiều lần
                aggregateRoot.ClearDomainEvents();

                return domainEvents;
            })
            .Select(domainEvent => new OutboxMessage
            {
                // Tạo ID mới cho OutboxMessage
                Id = Guid.NewGuid(),
                // Ghi lại thời điểm xảy ra sự kiện (UTC)
                OccurredOnUtc = DateTime.UtcNow,
                // Lấy tên kiểu của Domain Event
                Type = domainEvent.GetType().Name,
                // Chuyển Domain Event thành JSON để lưu trữ
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
            })
            .ToList();

        // Thêm các OutboxMessage vào DbContext (bảng Outbox)
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        // Tiếp tục lưu các thay đổi (với khả năng retry của EF Core nếu cần)
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
