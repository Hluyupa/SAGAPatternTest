using BrokerMessage;
using MassTransit;
using SomeService.Repositories;

namespace SomeService.Consumers;

public class CancelOrderConsumer : IConsumer<OrderMessage>
{
    private readonly OrderRepository _orderRepository;

    public CancelOrderConsumer(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    public async Task Consume(ConsumeContext<OrderMessage> context)
    {
        await _orderRepository.CancelOrderAsync(context.Message.Id);
    }
}