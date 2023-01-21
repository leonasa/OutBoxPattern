// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using FASTER.core;
using OrderProcessor;
using Shared.Contracts;
using Shared.InboxServices;
using Shared.OutboxServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IOrderReceivedHandler, OrderReceivedHandler>();

builder.Services.AddSingleton<IProducer<string?, OrderFulfilledMessage>>(sp =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    return new ProducerBuilder<string?, OrderFulfilledMessage>(config)
        .SetValueSerializer(new JsonSerializer<OrderFulfilledMessage>()).Build();
});
builder.Services.AddSingleton<IConsumer<string?, OrderMessage>>(sp =>
{
    var config = new ConsumerConfig
    {
        BootstrapServers = "localhost:9092",
        GroupId = "order-processor",
        AutoOffsetReset = AutoOffsetReset.Earliest
    };
    return new ConsumerBuilder<string?, OrderMessage>(config)
        .SetValueDeserializer(new JsonDeserializer<OrderMessage>().AsSyncOverAsync()).Build();
});

builder.Services.AddSingleton<IOrderOFulfilledProducer, OrderOFulfilledProducer>();
// builder.Services.AddSingleton<IOutboxStore<OrderFulfilledMessage>, OutboxStore<OrderFulfilledMessage>>();
builder.Services.AddSingleton<IInboxStore<OrderMessage>, InboxStore<OrderMessage>>();
builder.Services.AddSingleton<IOrderConsumer, KafkaOrderConsumer>();
builder.Services.AddHostedService<InboxConsumer>();
builder.Services.AddHostedService<InboxProcessor>();
builder.Services.AddHostedService<OutboxFulfilledProcessor>();

builder.Services.AddSingleton<FasterLog>(_ =>
{
    var path = Path.GetTempPath() + "FasterLogPubSub/";
    var device = Devices.CreateLogDevice(path + "OrderProcessor.log");
    var fasterLogSettings = new FasterLogSettings
    {
        LogDevice = device, 
        MemorySizeBits = 11, 
        PageSizeBits = 9, 
        MutableFraction = 0.5, 
        SegmentSizeBits = 9,
        RemoveOutdatedCommits = true, 
        AutoRefreshSafeTailAddress = true
    };
    return new FasterLog(fasterLogSettings);
});

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


//PRODUCE THE RESULT HERE TO THE OUTBOX