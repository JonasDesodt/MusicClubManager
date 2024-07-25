using MusicClubManager.Abstractions;
using MusicClubManager.Services;
using MusicClubManager.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.Extensions.Hosting;
using MusicClubManager.Api.Contexts;
using MusicClubManager.Core.Models;
using Microsoft.Extensions.Configuration;
using MusicClubManager.Services.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("https://localhost:7178")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MusicClubManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

//Configuration from AppSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

//User Manager Service
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<MusicClubManagerDbContext>();
       //.AddDefaultTokenProviders();

//Adding Athentication - JWT
builder.Services
    .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
        };
    });

builder.Services.AddScoped<IArtistService, ArtistDbService>();
builder.Services.AddScoped<IEventService, EventDbService>();
builder.Services.AddScoped<ImageDbService>();
builder.Services.AddScoped<ILineupService, LineupDbService>();
builder.Services.AddScoped<IPerformanceService, PerformanceDbService>();

builder.Services.AddScoped <IIdentityService, IdentityDbService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //using var scope = app.Services.CreateScope();
    //var services = scope.ServiceProvider;
    //var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    //try
    //{
    //    //Seed Default Users
    //    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    //    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    //    await ApplicationDbContextSeed.SeedEssentialsAsync(userManager, roleManager);
    //}
    //catch (Exception ex)
    //{
    //    var logger = loggerFactory.CreateLogger<Program>();
    //    logger.LogError(ex, "An error occurred seeding the DB.");
    //}
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();