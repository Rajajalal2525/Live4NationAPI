using System.Text;
using Live4Nation.BLL.Interfaces;
using Live4Nation.BLL.Services;
using Live4Nation.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext Registration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Services Registration
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAdvertisementService, AdvertisementService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IGalleryService, GalleryService>();
builder.Services.AddScoped<IVideoService, VideoService>();
builder.Services.AddScoped<IContactService, ContactService>();

// AuthService ko register kiya
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Configuration aur Authentication setup kiya
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Note: Swagger ko condition se bahar nikal diya hai taaki MonsterASP (Production) par bhi chale.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Live4Nation API V1");
});

app.UseHttpsRedirection();

// Authentication aur Authorization Middleware (MapControllers se pehle)
app.UseAuthentication(); 
app.UseAuthorization();

// Root URL (/) ko automatic Swagger par redirect karne ke liye fallback route
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.MapControllers();

app.Run();