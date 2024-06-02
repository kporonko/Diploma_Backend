using Diploma.Backend.Application.Repositories;
using Diploma.Backend.Application.Repositories.Payment;
using Diploma.Backend.Application.Repositories.Payment.Proxies;
using Diploma.Backend.Application.Repositories.Stats;
using Diploma.Backend.Application.Services;
using Diploma.Backend.Application.Services.impl;
using Diploma.Backend.Application.Services.Payment.impl;
using Diploma.Backend.Application.Services.Stats.impl;
using Diploma.Backend.Infrastructure.Data;
using Diploma.Backend.Infrastructure.PayPal.Facades;
using Diploma.Backend.Infrastructure.PayPal.Facades.impl;
using Diploma.Backend.Infrastructure.PayPal.Proxies;
using Diploma.Backend.Infrastructure.PayPal.Proxies.impl;
using Diploma.Backend.Infrastructure.PayPal.Repositories.impl;
using Diploma.Backend.Infrastructure.Repositories;
using Diploma.Backend.Infrastructure.Repositories.impl;
using Diploma.Backend.Infrastructure.Stats.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

string? connection = builder.Configuration.GetConnectionString("Default");

// Add services to the container.
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IUnitAppearanceService, UnitAppearanceService>();
builder.Services.AddTransient<ITemplateService, TemplateService>();
builder.Services.AddTransient<ITargetingService, TargetingService>();
builder.Services.AddTransient<ISurveyService, SurveyService>();
builder.Services.AddTransient<ISurveyUnitService, SurveyUnitService>();
builder.Services.AddTransient<IPayPalFacade, PayPalFacade>();
builder.Services.AddTransient<IRestClient, RestClient>();
builder.Services.AddTransient<IPaymentService, PaymentService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IStatsRetriever, StatsRetriever>();
builder.Services.AddTransient<IAuthRepository, AuthRepository>();
builder.Services.AddTransient<ISurveyRepository, SurveyRepository>();
builder.Services.AddTransient<ISurveyUnitRepository, SurveyUnitRepository>();
builder.Services.AddTransient<ITargetingRepository, TargetingRepository>();
builder.Services.AddTransient<ITemplateRepository, TemplateRepository>();
builder.Services.AddTransient<IUnitAppearanceRepository, UnitAppearanceRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IPaymentRepository, PayPalRepository>();
builder.Services.AddTransient<IPaymentProxy, PayPalProxy>();
builder.Services.AddTransient<IStatsRepository, StatsRepository>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:3000") // Replace with your frontend application URL
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                      });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddDbContext<ApplicationContext>(options =>
        options.UseSqlServer(connection));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
