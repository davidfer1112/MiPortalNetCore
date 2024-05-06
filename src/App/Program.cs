using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MiPortal;
using MiPortal.Data;
using MiPortal.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IEmailService, EmailService>();

var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();
startup.Configure(app, builder.Environment);

app.Run();
