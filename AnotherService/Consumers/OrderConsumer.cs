using AnotherService.Repositories;
using BrokerMessage;
using MassTransit;

namespace AnotherService.Consumers;

public class OrderConsumer : IConsumer<OrderMessage>
{
    private readonly AccountRepository _accountRepository;
    private readonly ITopicProducer<OrderMessage> _topicProducer;

    public OrderConsumer(
        AccountRepository accountRepository, 
        ITopicProducer<OrderMessage> topicProducer)
    {
        _accountRepository = accountRepository;
        _topicProducer = topicProducer;
    }
    
    public async Task Consume(ConsumeContext<OrderMessage> context)
    {
        if (await _accountRepository.TryPayAsync(context.Message.Cost))
        {
            return;
        }
        
        await _topicProducer.Produce(new
        {
            Id = context.Message.Id,
            Cost = context.Message.Cost
        });
    }
}