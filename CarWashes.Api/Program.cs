using CarWashes.Application.Services;
using CarWashes.Core.Interfaces;
using CarWashes.DataBase.Postgres;
using CarWashes.DataBase.Postgres.Repositories;
using CarWashes.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions)));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarWashesDbContext>(
	options =>
	{
		options.UseNpgsql(configuration.GetConnectionString(nameof(CarWashesDbContext)));
	});

builder.Services.AddScoped<IHumansService, HumansService>();
builder.Services.AddScoped<IHumansRepository, HumansRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ICarwashesService, CarwashesService>();
builder.Services.AddScoped<ICarwashesRepository, CarwashesRepository>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtOptions:SecretKey")))
		};
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				context.Token = context.Request.Cookies["milk-cookies"];
				return Task.CompletedTask;
			}
		};
	});
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
