using StackExchange.Redis;
using RestApi.Infrastructure;
using Infrastructure.Extensions;
using RestApi.Infrastructure.Caching.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerServices();
builder.Services.AddProductService();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect("redis"));

builder.Services.AddRedisOutputCache();
builder.Services.AddFluentMigratorServices(builder.Configuration);
builder.Services.AddAuthenticationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseOutputCache();
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Migrate();

app.Run();
