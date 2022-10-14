using System.Globalization;
using CommonLib.Extensions;
using CommonLib.StringLocalizer;
using Expense_Identity.Database;
using Expense_Identity.Services;
using Expense_Identity.Services.Interfaces;
using Expense_Identity.Utils;
using JwtIdentityLib;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;

// For Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Identity")));

// Json Location
builder.Services.AddJsonLocation();

builder.Services.AddDistributedMemoryCache();

// For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Adding Authentication
// Adding Jwt Bearer
builder.AddJwtIdentity();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "Expense Managerment APIs",
            Version = "v1"
        });
    options.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please Enter the valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// DI
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<CurrentUser>();

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
app.UseStaticFiles();
app.UseMiddleware<LocalizationMiddleware>();

app.UseCors(x => x.AllowAnyHeader()
      .AllowAnyMethod()
      .WithOrigins("http://localhost:3000"));

// Authentication & Authorization
//app.UseAuthentication();
//app.UseAuthorization();
app.UseJwtIdentity();

app.MapControllers();


app.Run();

