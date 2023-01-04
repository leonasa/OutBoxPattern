// See https://aka.ms/new-console-template for more information

using Confluent.Kafka;
using Shared.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(() =>
{
    var pConfig = new ProducerConfig { BootstrapServers = "kafka:9092" };
    return new ProducerBuilder<string?, OrderFulfilledMessage>(pConfig).Build();
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