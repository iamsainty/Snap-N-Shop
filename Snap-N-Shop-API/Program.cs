using Snap_N_Shop_API.Data;
using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Services;
using Snap_N_Shop_API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<EmailService>();

builder.Services.AddDbContext<MyDbContext>(
    options => options.UseSqlServer(connectionString)
);

var app = builder.Build();

app.MapCustomerEndpoints();
app.MapProductEndpoints();
app.MapCartEndpoints();
app.MapGet("/", () => "Hello from Snap-N-Shop API - backend is up hello");

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.Run();