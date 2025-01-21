using Microsoft.EntityFrameworkCore;
using Persistence;
using Service;
using Util.Error;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Repositories
builder.Services.AddPersistenceServices(builder.Configuration);

// Services
builder.Services.AddApplicationServices();

// Controllers
builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<YapidromDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

app.UseMiddleware<GlobalExceptionHandler>();

