using MassTransit;
using MediatR;

namespace DistributedSystem.Infrastructure.Consumer.Abstractions.Messages;

public abstract class Consumer<TMessage> : IConsumer<TMessage>
    where TMessage : class, Contract.Abstractions.Message.IDomainEvent
{
    private readonly ISender sender;
    protected Consumer(ISender sender)
    {
        this.sender = sender;
    }

    public async Task Consume(ConsumeContext<TMessage> context)
    {
        // send to ProjectProductDetailsWhenProductChangeEventHandler to handle
        await sender.Send(context.Message);
    }
}
