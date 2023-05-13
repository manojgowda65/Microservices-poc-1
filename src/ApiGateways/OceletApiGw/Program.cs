using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
 builder.Host.ConfigureLogging((hostingCtxt, loggingBuilder) =>
            {
               loggingBuilder.AddConfiguration(hostingCtxt.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

builder.Services.AddOcelot();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
await app.UseOcelot();

app.Run();
