using Snap_N_Shop_API.Data;
using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Services;
using Snap_N_Shop_API.Endpoints;
using Snap_N_Shop_API.Services.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<EmailService>();

builder.Services.AddDbContext<MyDbContext>(
    options => options.UseSqlServer(connectionString)
);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    await SeedData.Seed(dbContext);
}

app.UseCors("AllowAll");

app.MapCustomerEndpoints();
app.MapProductEndpoints();
app.MapCartEndpoints();
app.MapOrderEndpoints();
app.MapGet("/", () => "Hello from Snap-N-Shop API - backend is up hello");

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.Urls.Add("http://0.0.0.0:80");

app.Run();