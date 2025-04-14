using Infrastructure;
using Serilog;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers(options =>
{
   //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
   //options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowDev",
       policy => policy.WithOrigins("http://localhost:5173")
                       .AllowAnyMethod()
                       .AllowAnyHeader());
});

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("AllowDev");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await AuthDbInitializer.SeedDataAsync(app.Services);

app.Run();
