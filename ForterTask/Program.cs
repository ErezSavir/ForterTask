using Core.Interfaces;
using Core.Interfaces.Providers;
using Core.Services;
using Infrastructure.Adapters;
using Infrastructure.ExternalServices;
using Infrastructure.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ICryptoService, CryptoService>();
builder.Services.AddTransient<ICryptoProvider, CryptoAdapter>();
builder.Services.AddTransient<ICryptoCompareClient, CryptoCompareClient>();
builder.Services.Configure<CryptoApiSettings>(builder.Configuration.GetSection("CryptoApiSettings"));
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();