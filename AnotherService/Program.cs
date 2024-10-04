using AnotherService;
using AnotherService.Consumers;
using AnotherService.Repositories;
using BrokerMessage;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Context>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AccountRepository>();

builder.Services.AddMassTransit(p =>
{
    p.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
    p.AddRider(rider =>
    {
        const string kafkaBrokerServer = "localhost:9092";
    
        rider.AddProducer<OrderMessage>("order-cancel-topic");
        rider.AddConsumer<OrderConsumer>();
        
        rider.UsingKafka((context, k) =>
        {
            k.Host(kafkaBrokerServer);
            k.TopicEndpoint<OrderMessage>("order-topic", "order-consumer-group", e =>
            {
                e.ConfigureConsumer<OrderConsumer>(context);
            });
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();