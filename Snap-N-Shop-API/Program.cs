var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Simple health / test endpoint
app.MapGet("/", () => "Hello from Snap-N-Shop API - backend is up");

// Simple JSON endpoint to show minimal API usage
app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }));

app.Run();