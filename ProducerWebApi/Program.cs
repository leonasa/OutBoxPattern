using Confluent.Kafka;
using FASTER.core;
using OutBoxPattern;
using Shared.Contracts;
using Shared.OutboxServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IOrderProducer, OrderProducer>();
builder.Services.AddSingleton<IProducer<string?, OrderMessage>>(_ =>
{
    var config = new ProducerConfig
    {
        BootstrapServers = "localhost:9092"
    };
    return new ProducerBuilder<string?, OrderMessage>(config)
        .SetValueSerializer(new JsonSerializer<OrderMessage>()).Build();
});


builder.Services.AddSingleton<FasterLog>(_ =>
{
    var path = Path.GetTempPath() + "FasterLogPubSub/";
    var device = Devices.CreateLogDevice(path + "ProducerWebApi.log");
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