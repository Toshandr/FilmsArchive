using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using MovieAPI.Middleware;
using FilmsArchive.Models;
using StackExchange.Redis;
using MovieAPI.Cache;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
var connectionString = Environment.GetEnvironmentVariable("SUPABASE_STRING"); //подгружаем строку подключения к postgre из .env
var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "localhost:6379"; ////подгружаем строку подключения к redis из .env

builder.Services.AddDbContext<postgresContext>(options =>
    options.UseNpgsql(connectionString));

try
{
    var muxer = ConnectionMultiplexer.Connect(redisConnectionString);
    builder.Services.AddSingleton<IConnectionMultiplexer>(muxer);
    builder.Services.AddSingleton<RedisCache>();
}
catch (Exception ex)
{
    Console.WriteLine($"Redis connection failed: {ex.Message}");
}

builder.Services.AddScoped<MovieAPI.Services.ActorService>(); //регистрируем класс для работы с актёрами
builder.Services.AddScoped<MovieAPI.Services.MovieService>(); //регистрируем класс для работы с фильмами

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();