using AuthenticationService.API;
using AuthenticationService.API.Repository;
using AuthenticationService.API.Resources;
using AuthenticationService.API.Services;
using AuthenticationService.API.Validators;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

UserResource.InitializeUserData();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JwtOptions"));

builder.Services.AddSingleton<IAuthenticationService, AuthenticationService.API.Services.AuthenticationService>();
builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();
    
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
