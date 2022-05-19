using System.Text;
using BasketService.Application;
using BasketService.Application.Configuration;
using BasketService.Application.Helper;
using BasketService.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<MongoConfig>(builder.Configuration.GetSection("MongoConfig"));

builder.Services.RegisterApplicationServices();
builder.Services.RegisterPersistenceServices();

#region Authentication
AuthTokenOptions jwtOptions = builder.Configuration.GetSection("AuthTokenOptions").Get<AuthTokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateActor = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey))
    };
});
#endregion

var app = builder.Build();

var dataInitializer = app.Services.GetRequiredService<DataInitializer>();
dataInitializer.InitializeBasketData();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();