using System.Globalization;
using _2.ExpenseManagement.Api.Database;
using _2.ExpenseManagement.Api.Services.Attachments;
using _2.ExpenseManagement.Api.Services.Categories;
using _2.ExpenseManagement.Api.Services.Expenses;
using _2.ExpenseManagement.Api.Services.ExpenseTypes;
using _2.ExpenseManagement.Api.Services.File;
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

var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{env}.json", optional: true);
builder.Host.UseSerilog();

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

//create the logger and setup your sinks, filters and properties
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateBootstrapLogger();

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
builder.Services.AddScoped<IExpenseTypeService, ExpenseTypeService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

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

