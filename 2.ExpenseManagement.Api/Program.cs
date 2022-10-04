using System.Globalization;
using _2.ExpenseManagement.Api.Database;
using _2.ExpenseManagement.Api.Services.Categories;
using _2.ExpenseManagement.Api.UoW;
using CommonLib.Extensions;
using CommonLib.Middlewares;
using CommonLib.Services;
using CommonLib.StringLocalizer;
using JwtIdentityLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;

//create the logger and setup your sinks, filters and properties
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddJsonLocation();

builder.Services.AddDistributedMemoryCache();

// Adding Authentication
// Adding Jwt Bearer
builder.AddJwtIdentity();

// For Entity Framework
builder.Services.AddDbContext<ExpenseContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("ExpenseDb")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var options = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(new CultureInfo("en-US"))
};
app.UseRequestLocalization(options);
app.UseMiddleware<LocalizationMiddleware>();
app.UseMiddleware<RequestJwtMiddleware>();

app.UseCors(x => x.AllowAnyHeader()
      .AllowAnyMethod()
      .WithOrigins("http://localhost:3000"));

//app.UseJwtIdentity();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

