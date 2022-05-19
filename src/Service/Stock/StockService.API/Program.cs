using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StockService.API.Validators;
using StockService.Business;
using StockService.DataAccess;
using StockService.DataAccess.Cache;
using StockService.Entity.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(config =>
{
    config.RegisterValidatorsFromAssemblyContaining<CheckStockAvailabilityRequestValidator>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("RedisOptions"));

builder.Services.AddSingleton<IStockService, StockService.Business.StockService>();
builder.Services.AddSingleton<IStockRepository, CacheStockRepository>();
builder.Services.AddSingleton<ICache>(provider => 
{
    //We are connecting to redis once
    RedisOptions redisConfig = provider.GetRequiredService<IOptions<RedisOptions>>().Value;
    RedisCache redisCache = new RedisCache(redisConfig);
    redisCache.InitializeConnection().GetAwaiter().GetResult();
    return redisCache;
});

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

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();