using MultiShop.Discount.Context;
using MultiShop.Discount.Controllers;
using MultiShop.Discount.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Constructor injection ile kullanýlan dependency’leri DI container’a register etmelisin.
// Yani DiscountService constructor methodu parametre olarak DapperContext alýyor,
// o yüzden DapperContext'i de register etmelisin. Aksi takdirde runtime error alýrsýn.
// Çünkü dependency injection container, DapperContext'i nasýl oluþturacaðýný bilmez.

//Controller þöyleyse:public DiscountController(IDiscountService discountService) 
// DI container:“IDiscountService interface’inin concrete implementasyonu ne?” bilmek zorunda.

builder.Services.AddTransient<DapperContext>();

//IDiscountService istendiðinde DiscountService üret
builder.Services.AddTransient<IDiscountService, DiscountService>();


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
