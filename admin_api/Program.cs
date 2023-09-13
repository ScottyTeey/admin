using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;

// Create a new web application builder instance.
var builder = WebApplication.CreateBuilder(args);

// Enable Cross-Origin Resource Sharing (CORS) to allow requests from any origin, method, and header.
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Configure JSON serialization settings using Newtonsoft.Json.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    // Ignore reference loops in JSON serialization.
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    // Use the DefaultContractResolver for JSON serialization.
    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

// Add services to the container for MVC controllers.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI documentation generation.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Build the web application.
var app = builder.Build();

// Enable CORS for all requests.
app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline based on the environment.

if (app.Environment.IsDevelopment())
{
    // Use Swagger and Swagger UI for API documentation in the development environment.
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable authorization (if needed for your application).
app.UseAuthorization();

// Map controllers to routes.
app.MapControllers();

// Start serving the application.
app.Run();

// Serve static files from a "Photos" directory.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});
