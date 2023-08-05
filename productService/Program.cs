using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using productService.Domain.Repositories.Interfaces;
using productService.Api.Services;
using productService.Infra.Context;
using System.Text.Json.Serialization;
using productService.Infra.Repositories;
using productService.Api.Services;
using productService.Api.Services.Interfaces;
using productService.Api.Services.Caching;
using productService.Api.Services.Caching.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors();

builder.Services.AddResponseCompression(rp => 
{
    rp.Providers.Add<GzipCompressionProvider>();
    rp.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
});

// builder.Services.AddResponseCaching();

builder.Services.AddControllers().AddJsonOptions(jo =>
                jo.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var secretKey = Encoding.ASCII.GetBytes(config["Jwt:Key"]);
builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jb =>
{
    jb.RequireHttpsMetadata = true;
    jb.SaveToken = true;
    jb.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        ClockSkew = TimeSpan.Zero
    };
});

services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOrEmployee", policy => policy.RequireRole("manager", "employee"));
});

builder.Services.AddDbContext<AppDbContext>(adc => 
    adc.UseInMemoryDatabase("Database"));

// var configuration = new ConfigurationBuilder()
//     .SetBasePath(Directory.GetCurrentDirectory())
//     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
//     .Build();

// var connectionString = configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<AppDbContext>(adc => 
//    adc.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICachingService, CachingService>();

builder.Services.AddStackExchangeRedisCache(serc => {
    serc.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    serc.InstanceName = "instance";
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sg =>
    sg.SwaggerDoc("v1", new OpenApiInfo { Title = "product Service API", Version = "v1",}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(su =>
    su.SwaggerEndpoint("/swagger/v1/swagger.json", "product Service - V1"));
}

app.UseHttpsRedirection();

app.UseCors(c =>
{
    c.AllowAnyOrigin();
    c.AllowAnyMethod();
    c.AllowAnyHeader();
});

app.UseAuthentication();        

app.UseAuthorization();

app.MapControllers();

app.Run();
