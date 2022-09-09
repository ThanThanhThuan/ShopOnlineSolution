using Blazored.LocalStorage;
using Blazored.Toast;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopOnline.Web;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contracts;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7069/") });

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
//--
builder.Services.AddScoped<IProductReceiptService, ProductReceiptService>();
builder.Services.AddScoped<IProductReceiptDetailService, ProductReceiptDetailService>();
builder.Services.AddScoped<ISupplierService, SupplierService>();

builder.Services.AddScoped<IToastService, ToastService>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredToast();

builder.Services.AddScoped<IManageProductsLocalStorageService, ManageProductsLocalStorageService>();
builder.Services.AddScoped<IManageCartItemsLocalStorageService, ManageCartItemsLocalStorageService>();

await builder.Build().RunAsync();
