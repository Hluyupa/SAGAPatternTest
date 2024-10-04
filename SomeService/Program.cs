using BrokerMessage;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SomeService;
using SomeService.Consumers;
using SomeService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<Context>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<OrderRepository>();

builder.Services.AddMassTransit(p =>
{
    p.UsingInMemory((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
    p.AddRider(rider =>
    {
        const string kafkaBrokerServer = "localhost:9092";
    
        rider.AddProducer<OrderMessage>("order-topic");
        rider.AddConsumer<CancelOrderConsumer>();
        
        rider.UsingKafka((context, k) =>
        {
            k.Host(kafkaBrokerServer);
            k.TopicEndpoint<OrderMessage>("order-cancel-topic", "cancel-order-consumer-group", e =>
            {
                e.ConfigureConsumer<CancelOrderConsumer>(context);
            });
        });
    });
});

var app = builder.Build();

app.UseRouting();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();