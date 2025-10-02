using Snap_N_Shop_API.Data;
using Microsoft.EntityFrameworkCore;
using Snap_N_Shop_API.Services;
using Snap_N_Shop_API.Endpoints;
using Snap_N_Shop_API.Services.Data;
using Snap_N_Shop_API.Services.AuthToken;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["connectionString"];
var jwtKey = builder.Configuration["jwtkey"];
var jwtIssuer = builder.Configuration["jwtissuer"];
var jwtAudience = builder.Configuration["jwtaudience"];
var smtpUser = builder.Configuration["smtpusername"];
var smtpPass = builder.Configuration["smtppass"];

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.Configure<SmtpSettings>(options =>
{
    options.UserName = smtpUser ?? string.Empty;
    options.Password = smtpPass ?? string.Empty;
});
builder.Services.Configure<JwtSettings>(options =>
{
    options.SecretKey = jwtKey ?? string.Empty;
    options.Issuer = jwtIssuer ?? string.Empty;
    options.Audience = jwtAudience ?? string.Empty;
});
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