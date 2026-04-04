using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MultiShop.Catalog.Services.AboutService;
using MultiShop.Catalog.Services.BrandService;
using MultiShop.Catalog.Services.CategoryService;
using MultiShop.Catalog.Services.ContactService;
using MultiShop.Catalog.Services.FeatureService;
using MultiShop.Catalog.Services.FeatureSliderService;
using MultiShop.Catalog.Services.OfferDiscountService;
using MultiShop.Catalog.Services.ProductDetailService;
using MultiShop.Catalog.Services.ProductImageService;
using MultiShop.Catalog.Services.ProductService;
using MultiShop.Catalog.Services.SpecialOfferService;
using MultiShop.Catalog.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
{
    //IdentityServerUrl appsettings.json içinden gelicek. OenID'i çaðýrýcaðýmýz yer. Bu token'ý kiminle kullanacaðýz
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.RequireHttpsMetadata = false;
    opt.Audience = "ResourceCatalog";
});

// Add services to the container.
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();
builder.Services.AddScoped<IProductImageService, ProductImageService>();
builder.Services.AddScoped<IFeatureSliderService, FeatureSliderService>();
builder.Services.AddScoped<ISpecialOfferService, SpecialOfferService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IOfferDiscountService, OfferDiscountService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IDatabaseSettings>(sp =>
    {
        return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
    });

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

app.UseAuthorization();

app.MapControllers();

app.Run();
