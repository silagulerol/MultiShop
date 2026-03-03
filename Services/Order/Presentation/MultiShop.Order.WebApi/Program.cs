using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using MultiShop.Order.Application.Features.CQRS.Handlers.AddressHandlers;
using MultiShop.Order.Application.Features.CQRS.Handlers.OrderDetailHandlers;
using MultiShop.Order.Application.Interfaces;
using MultiShop.Order.Application.Services;
using MultiShop.Order.Persistance.Context;
using MultiShop.Order.Persistance.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(opt =>
{
    var authority = builder.Configuration["IdentityServerUrl"]; // http://localhost:5001

    opt.RequireHttpsMetadata = false;
    opt.Authority = authority;
    opt.MetadataAddress = $"{authority}/.well-known/openid-configuration";
    opt.Audience = "ResourceOrder";

    opt.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
        opt.MetadataAddress,
        new OpenIdConnectConfigurationRetriever(),
        new HttpDocumentRetriever { RequireHttps = false }
    );

    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authority,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudiences = new[] { "ResourceOrder" }
    };

   
});
builder.Services.AddDbContext<OrderContext>();


// Add services to the container.

//---- Open Generic Registration, Tek kayýtla tüm T’ler----
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//-----MediatR design pattern registration ---------
builder.Services.AddApplicationService(builder.Configuration);


//----- Sadece Concrete Type register ettik ----------
#region
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
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
