using Threads.Api;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();

var app = builder.Build();

app.MapGet("/api", () => "Hello World!");
app.Configure();
app.Run();
