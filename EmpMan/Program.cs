using EmpMan.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using EmpMan.Data;
using Microsoft.OpenApi.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Configure Entity Framework and SQL Server
builder.Services.AddDbContext<EmpManDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("EmpManDBContext") ?? throw new InvalidOperationException("Connection string 'EmpManDBContext' not found.")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register HttpClient with the base URL from appsettings.json
builder.Services.AddHttpClient("EmpManApiClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"]); // Ensure this URL is correct
});

// Add services to the container
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new IgnoreAntiforgeryTokenAttribute()); // Disable anti-forgery validation for API controllers
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EmpMan API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// **Add back UseAntiforgery to support Blazor forms**
app.UseAntiforgery();

// Configure authentication and authorization
app.UseAuthentication();  // Ensure this is called before UseAuthorization
app.UseAuthorization();

// Map Swagger endpoints
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmpMan API V1");
    c.RoutePrefix = "swagger"; // Set this to the desired URL path for Swagger UI
});

// Map Razor components (Blazor pages)
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map API controllers
app.MapControllers();

app.Run();
