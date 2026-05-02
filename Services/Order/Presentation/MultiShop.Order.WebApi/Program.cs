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
    Modulus = Base64UrlEncoder.DecodeBytes("xygPrLAdgaS-MRZ8w2UnMP1NPJWzLdVk1v6O4W52BG-5wWypFzxUeJJd6BtzcGO5a2mWv8MMUvgne2bMNk0XcLKN__mQ41T29fjFrep_PXppUdV0EDeA5IoUuan98_gOxUauXXOtwTiPmXN94Eli3qYY-qpQZL-xs_Nhm5BlIKoEm4b27KisIVAxsNg_WZ5JbPyr0QHXkRm8mW2mByAqbiHiyKSyIBIq6i5JswsZehttoki3P93OISvECiVB-bupdBTIbvm39ovXWF863EVg4scPbm6f-7YDkbBFmcuZMn_iPrFXYGLOC9eGlADbSJqeAp3NSpM5Jew3UWnwRbRfjQ"),
    Exponent = Base64UrlEncoder.DecodeBytes("AQAB")
});

var securityKey = new RsaSecurityKey(rsa)
{
    KeyId = "3EE6973FCFCBE7A3E351E91D36D87526"
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
builder.Services.AddScoped(typeof(IOrderingRepository), typeof(OrderingRepository));
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