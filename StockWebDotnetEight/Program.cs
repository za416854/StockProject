using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
// JWT 驗證相關參考
using System.Text;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens; 
using DataAccess;
using StockWebDotnetEight.Services.TaiwanStock;
using StockWebDotnetEight.Services;
using DataAccess.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 使用能存取HttpContext的物件
builder.Services.AddHttpContextAccessor();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
/* ----------------------------------------------------------------------------
   註冊Dapper相關的物件
---------------------------------------------------------------------------- */
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ICommonRepository, CommonRepository>();
builder.Services.AddScoped<IDailyTaiwanStockRepository, DailyTaiwanStockRepository>();


/* ----------------------------------------------------------------------------
   註冊服務相關的物件
---------------------------------------------------------------------------- */
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IDailyTaiwanStockService, DailyTaiwanStockService>();
// 換用NewtonsoftJson處理JSON;
builder.Services.AddControllers().AddNewtonsoftJson();


var app = builder.Build();  
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler("/api/error");

app.UseHttpsRedirection();
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};
//app.MapGet("/", () => $"{appTitle} 版本: {appVersion}");
//app.MapGet("/", () => "api/DailyTaiwanStock/TestGet");
app.MapControllers();
app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();
app.UseRouting(); 


app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
