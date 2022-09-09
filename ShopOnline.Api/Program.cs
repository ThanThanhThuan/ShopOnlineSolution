using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories;
using ShopOnline.Api.Repositories.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); //zz comm
builder.Services.AddSwaggerGen();  //zz comm

//***IMPORTANT INSTRUCTION HERE - MUST CONFIGURE CONNECTION BEFORE RUNNING MIGRATIONS
builder.Services.AddDbContextPool<ShopOnlineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShopOnlineConnection"))
);

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
//zz
builder.Services.AddScoped<IProductReceiptRepository, ProductReceiptRepository>();
builder.Services.AddScoped<IProductReceiptDetailRepository, ProductReceiptDetailRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() )   //zz Comm- false
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.WithOrigins("http://localhost:7060", "https://localhost:7060")
    .AllowAnyMethod()
    .WithHeaders(HeaderNames.ContentType)
);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
