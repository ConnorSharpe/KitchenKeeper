using KitchenKeeper.BAL.Stock_BAL;
using KitchenKeeper.DAL.Stock_SQL;
using Microsoft.Data.SqlClient;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//AddInterfaces
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStock_SQL, Stock_SQL>();

//AddDB
builder.Services.AddTransient<IDbConnection>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new SqlConnection(config.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
