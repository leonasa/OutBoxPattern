using Confluent.Kafka;
using OutBoxPattern;
using Shared.Contracts;
using Shared.OutboxServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IOrderProducer, OrderProducer>();
builder.Services.AddSingleton<IOutboxStore<OrderMessage>, OutboxStore<OrderMessage>>();
builder.Services.AddSingleton<IProducer<string?, OrderMessage>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    return new ProducerBuilder<string?, OrderMessage>(config)
        .SetValueSerializer(new JsonSerializer<OrderMessage>()).Build();
});


builder.Services.AddHostedService<OutboxProcessor>();

builder.Services.Configure<OutboxOptions>(
    builder.Configuration.GetSection(OutboxOptions.ConfigurationSectionName));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();