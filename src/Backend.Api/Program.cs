using Backend.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddBackendServices(builder.Configuration);

var app = builder.Build();
app.MapBackendEndpoints();

app.Run();

public partial class Program;
