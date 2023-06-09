using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((hostCtxt, cfg) =>
{
    cfg.AddJsonFile($"Ocelot.{hostCtxt.HostingEnvironment.EnvironmentName}.json",true,true);

}).ConfigureLogging((hostingCtxt, loggingBuilder) =>
            {
               loggingBuilder.AddConfiguration(hostingCtxt.Configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

builder.Services.AddOcelot().AddCacheManager(x=>x.WithDictionaryHandle());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
await app.UseOcelot();

app.Run();
