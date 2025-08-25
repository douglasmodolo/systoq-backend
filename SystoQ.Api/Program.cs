using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using SystoQ.Api.DTOs.Mappings;
using SystoQ.Api.Middlewares;
using SystoQ.Application.UseCases.Customers;
using SystoQ.Application.UseCases.Products;
using SystoQ.Application.UseCases.Sales;
using SystoQ.Domain.Filters;
using SystoQ.Domain.Repositories;
using SystoQ.Infrastructure.Persistence;
using SystoQ.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL connection
builder.Services.AddDbContext<SystoQDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<AddProductUseCase>();
builder.Services.AddScoped<GetAllProductsUseCase>();
builder.Services.AddScoped<GetProductByIdUseCase>();
builder.Services.AddScoped<GetProductsByPriceRangeUseCase>();
builder.Services.AddScoped<UpdateProductUseCase>();
builder.Services.AddScoped<PatchProductUseCase>();
builder.Services.AddScoped<DeleteProductUseCase>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<AddCustomerUseCase>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<AddSaleUseCase>();

builder.Services.AddAutoMapper(typeof(ProductDTOMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigureExceptionHandler(); // Use custom exception handler in development
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
