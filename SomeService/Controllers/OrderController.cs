using BrokerMessage;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SomeService.Repositories;

namespace SomeService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ITopicProducer<OrderMessage> _topicProducer;
    private readonly OrderRepository _orderRepository;

    public OrderController(
        ITopicProducer<OrderMessage> topicProducer,
        OrderRepository orderRepository)
    {
        _topicProducer = topicProducer;
        _orderRepository = orderRepository;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateOrder(int cost)
    {
        var order = await _orderRepository.CreateOrderAsync(cost);
        
        await _topicProducer.Produce(new
        {
            Id = order.Id,
            Cost = order.Cost
        });
        return Ok();
    } 
}