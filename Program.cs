using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Draft15.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Draft15Context>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Draft15Context") ?? throw new InvalidOperationException("Connection string 'Draft15Context' not found.")));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
