using Infrastructure.Extensions;
using RestApi.Infrastructure;
using RestApi.Infrastructure.Caching.Extensions;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddProductService();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("redis"));
builder.Services.AddRedisOutputCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOutputCache();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
