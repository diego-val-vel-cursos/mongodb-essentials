using Practice.Services.mslogs.Messaging;
using Practice.Services.mslogs.Models;
using Practice.Services.mslogs.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios para MongoDB
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

builder.Services.AddSingleton<LogService>();

// Configurar servicios para Kafka
builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection(nameof(KafkaSettings)));

builder.Services.AddSingleton<KafkaConsumer>();

builder.Services.AddControllers();

// Añadir Swagger al proyecto
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Iniciar el consumidor de Kafka en segundo plano
var kafkaConsumer = app.Services.GetRequiredService<KafkaConsumer>();
var cancellationTokenSource = new CancellationTokenSource();
Task.Run(() => kafkaConsumer.Consume(cancellationTokenSource.Token));

app.UseAuthorization();
app.MapControllers();

// Ejecutar la aplicación en HTTP en un puerto específico
app.Run("http://0.0.0.0:5000");
