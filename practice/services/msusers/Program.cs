﻿using Practice.Services.msusers.Messaging;
using Practice.Services.msusers.Models;
using Practice.Services.msusers.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Configurar servicios para MongoDB
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

builder.Services.AddSingleton<UserService>();

// Configurar servicios para Kafka
builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection(nameof(KafkaSettings)));

builder.Services.AddSingleton<KafkaProducer>();

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

app.UseAuthorization();
app.MapControllers();

// Ejecutar la aplicación en HTTP en un puerto específico
app.Run("http://0.0.0.0:5000");
