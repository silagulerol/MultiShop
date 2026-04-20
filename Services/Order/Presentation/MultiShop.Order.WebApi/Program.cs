using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers;
using MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Application.Services;
using MultiShop.Order.Persistance.Context;
using MultiShop.Order.Persistance.Repositories;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// JWKS'den gelen public key
var rsa = RSA.Create();
rsa.ImportParameters(new RSAParameters
{
    Modulus = Base64UrlEncoder.DecodeBytes("p_JJ3MUyIf3Sc2Ns08PytzwHBKvdbmVwPRl_QuDnVVFI-1AU8VV7jlnzb5dWl6Efl0Lr6xhrI6iticTVIu_ogwW-jVzIEe-0gTgkirVPDZ1YexKZY_fJVmqfY5QOJ01OawOUwi6oQP2Wj7iBXLy66tfXfwYH0oIWqEXJdYosfQVEcS-X5eSCk3AbipOsrfjTNK3b5tN915Yph9_nzFl6ZlDw-GSFWCwB69WXAVL08frBFwhNuXCKHDAwsNLTxodP5TpBxBW_5_whmxA5YItCxeSjbN-oGyypXNwSBqoIjrlTsPhIzvlAZDeUaTT-7oRwmHS7m8hzVd2YJZurB5nI5Q"),
    Exponent = Base64UrlEncoder.DecodeBytes("AQAB")
});

var securityKey = new RsaSecurityKey(rsa)
{
    KeyId = "E07468C1864DCEC8754F6256F7F519DF"
};

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:5001",

            ValidateAudience = false,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = securityKey
        };

    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<OrderContext>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddApplicationService(builder.Configuration);

builder.Services.AddScoped<GetAddressByIdQueryHandler>();
builder.Services.AddScoped<GetAddressQueryHandler>();
builder.Services.AddScoped<CreateAddressCommandHandler>();
builder.Services.AddScoped<UpdateAddressCommandHandler>();
builder.Services.AddScoped<RemoveAddressCommandHandler>();

builder.Services.AddScoped<GetOrderDetailByIdQueryHandler>();
builder.Services.AddScoped<GetOrderDetailQueryHandler>();
builder.Services.AddScoped<CreateOrderDetailCommandHandler>();
builder.Services.AddScoped<UpdateOrderDetailCommandHandler>();
builder.Services.AddScoped<RemoveOrderDetailCommandHandler>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // kapali kalsin

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();