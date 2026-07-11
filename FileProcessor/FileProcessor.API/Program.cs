using FileProcessor.API.Configurations;
using FileProcessor.API.Middleware;
using FileProcessor.Application.Interfaces;
using FileProcessor.Application.Services;
using FileProcessor.Domain.Interfaces;
using FileProcessor.Infrastructure;
using FileProcessor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FileProcessorDbContext>(options => { 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<APIKeyOptions>(
    builder.Configuration.GetSection("ApiKey"));

builder.Services.AddScoped<IFileProcessorRepository, FileProcessorRepository>();
builder.Services.AddScoped<IFileProcessorService, FileProcessorService>();  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ApiKeyMiddleware>();

app.MapControllers();

app.Run();
