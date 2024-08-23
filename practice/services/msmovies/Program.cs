using Movies.Models;
using Movies.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

builder.Services.AddSingleton<MovieService>();


builder.Services.AddControllers();


// Añadir Swagger al proyecto
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// var consumerService = app.Services.GetRequiredService<KafkaConsumerService>();
// Task.Run(() => consumerService.ConsumeMessages("test-topic"));



// Comment or remove the following line to disable HTTPS redirection
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Specify to run the application on HTTP and on a specific port
app.Run("http://0.0.0.0:5003");
