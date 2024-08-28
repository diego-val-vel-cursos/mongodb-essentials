using Practice.Services.msusers.Models;
using Practice.Services.msusers.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("msmovies", client =>{
    client.BaseAddress = new Uri("http://msmovies:5000"); // Usa el nombre del contenedor como host
});

// Add services to the container.
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));

builder.Services.AddSingleton<MongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

builder.Services.AddSingleton<UserService>();
// Registrar MovieClientService
builder.Services.AddSingleton<MovieClientService>();

builder.Services.AddControllers();

// Añadir Swagger al proyecto
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

// Specify to run the application on HTTP and on a specific port
app.Run("http://0.0.0.0:5000");
