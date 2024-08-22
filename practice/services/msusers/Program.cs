using Users.Models;
using Users.Services;
using Microsoft.Extensions.Options;
// using Confluent.Kafka;
// using KafkaExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

builder.Services.AddSingleton<UserService>();

// Configurar Kafka Producer
// var producerConfig = new ProducerConfig { BootstrapServers = "localhost:9092" };
// builder.Services.AddSingleton(producerConfig);
// builder.Services.AddSingleton<KafkaProducerService>();

// Configurar Kafka Consumer
// var consumerConfig = new ConsumerConfig
// {
//     GroupId = "test-group",
//     BootstrapServers = "localhost:9092",
//     AutoOffsetReset = AutoOffsetReset.Earliest
// };
// builder.Services.AddSingleton(consumerConfig);
// builder.Services.AddSingleton<KafkaConsumerService>();

builder.Services.AddControllers();

// Añadir Swagger al proyecto
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// var consumerService = app.Services.GetRequiredService<KafkaConsumerService>();
// Task.Run(() => consumerService.ConsumeMessages("test-topic"));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comment or remove the following line to disable HTTPS redirection
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Specify to run the application on HTTP and on a specific port
app.Run("http://0.0.0.0:5000");
