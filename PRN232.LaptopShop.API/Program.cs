using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PRN232.LaptopShop.API.Controllers;
using PRN232.LaptopShop.Repository;
using PRN232.LaptopShop.Repository.Entity;
using PRN232.LaptopShop.Repository.Repo;
using PRN232.LaptopShop.Service;
using System.Security.Claims;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "LootCutAI API", Version = "v1" });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    option.SchemaGeneratorOptions.SchemaIdSelector = type => type.FullName;
}
);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["SecretKey"])),
            NameClaimType = ClaimTypes  .NameIdentifier,
            RoleClaimType = ClaimTypes.Role
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();

                if (context.Response.HasStarted)
                    return;

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    IsSuccess = false,
                    Message = "Unauthorized or missing token"
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            },

            OnAuthenticationFailed = async context =>
            {
                if (context.Response.HasStarted)
                    return;

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var message = context.Exception?.GetType().Name switch
                {
                    "SecurityTokenExpiredException" => "Token expired",
                    "SecurityTokenInvalidSignatureException" => "Invalid token signature",
                    _ => "Invalid token"
                };

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    IsSuccess = false,
                    Message = message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            },

            OnForbidden = async context =>
            {
                if (context.Response.HasStarted)
                    return;

                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    IsSuccess = false,
                    Message = "You do not have permission to access this resource"
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        };
    });
//add db context
builder.Services.AddDbContext<ShopDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//add scope service
builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AccountRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<CategoryRepo>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    app.UseSwagger();
    app.UseSwaggerUI();
});

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
