using System.Text;
using BasketService.Application;
using BasketService.Application.Configuration;
using BasketService.Application.Helper;
using BasketService.Infrastructure;
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
builder.Services.RegisterInfrastructureServices();

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

#region HttpClient

builder.Services.AddHttpClient("StockService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("StockServiceUrl").Value);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("Resource", "BasketService");
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