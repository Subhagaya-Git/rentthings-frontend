using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RentThings.Api.Configuration;
using RentThings.Api.Data;
using RentThings.Api.Extensions;
using RentThings.Api.Hubs;
using RentThings.Api.Services;
using System.Text;
using RentThings.Api.Services.Azure;

var builder = WebApplication.CreateBuilder(args);

// 📊 Application Insights Telemetry සේවාව සක්‍රීය කිරීම
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
});

var azure = builder.Configuration.GetSection(AzureSettings.SectionName).Get<AzureSettings>() ?? new AzureSettings();
var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase");
var connectionString = azure.Sql.ConnectionString;

if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = "Server=(localdb)\\mssqllocaldb;Database=RentThings;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
}

if (useInMemory)
    builder.Services.AddDbContext<RentThingsDbContext>(o => o.UseInMemoryDatabase("RentThingsDev"));
else
    builder.Services.AddDbContext<RentThingsDbContext>(o => o.UseSqlServer(connectionString));

// Azure services (real or mock based on Azure:Integration feature flags)
builder.Services.AddRentThingsAzureServices(builder.Configuration);
builder.Services.AddRentThingsSignalR(builder.Configuration);

// Application Business Services
builder.Services.AddScoped<IListingService, ListingService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<ITrustScoreService, TrustScoreService>();

// 📱 Azure Communication Services (SMS/Email) සඳහා Notification Service එකතු කිරීම
builder.Services.AddTransient<INotificationService, NotificationService>();

var jwtSecret = azure.EntraId.ClientSecret.Length >= 32
    ? azure.EntraId.ClientSecret
    : "RentThings-Dev-Secret-Key-32chars!!";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = string.IsNullOrWhiteSpace(azure.EntraId.ClientId) ? "rentthings-dev" : azure.EntraId.ClientId,
            ValidateAudience = true,
            ValidAudience = string.IsNullOrWhiteSpace(azure.EntraId.ClientId) ? "rentthings-dev" : azure.EntraId.ClientId,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
            ValidateLifetime = true
        };
        // Allow JWT via query string for SignalR WebSocket connections
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(token) && context.Request.Path.StartsWithSegments("/hubs"))
                    context.Token = token;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IImageValidationService, ImageValidationService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RentThingsDbContext>();
    await db.Database.EnsureCreatedAsync();
    await DbSeeder.SeedAsync(db);
}

if (app.Environment.IsDevelopment())
    app.MapOpenApi();

app.UseCors("Frontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/notifications");

app.Run();