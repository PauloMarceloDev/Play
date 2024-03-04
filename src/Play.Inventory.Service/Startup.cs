using Microsoft.OpenApi.Models;
using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.Inventory.Service.Clients;
using Play.Inventory.Service.Entities;
using Polly;
using Polly.Timeout;

namespace Play.Inventory.Service;

public sealed class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration => configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMongo()
            .AddMongoRepository<InventoryItem>("inventoryItems")
            .AddMongoRepository<CatalogItem>("catalogItems")
            .AddMassTransitWithRabbitMq();

        // AddCatalogClient(services);

        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });
        
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Play.Inventory.Service", Version = "v1" });
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => 
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Inventory.Service v1"));
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
    
    private static void AddCatalogClient(IServiceCollection services)
    {
        var random = new Random();
        services.AddHttpClient<CatalogClient>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:5001");
            })
            .AddTransientHttpErrorPolicy(builder =>
                builder.Or<TimeoutRejectedException>().WaitAndRetryAsync(
                    5, 
                    retryAttempt => 
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(random.Next(0, 1000)),
                    onRetry: (outcome, timeSpan, retryAttempt) =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        serviceProvider.GetRequiredService<ILogger<CatalogClient>>()
                            .LogWarning($"Delaying for {timeSpan.TotalSeconds} seconds, then " +
                                        $"making retry {retryAttempt.Count}");
                    }))
            .AddTransientHttpErrorPolicy(builder =>
                builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
                    3,
                    TimeSpan.FromSeconds(15),
                    onBreak: (outcome, timeSpan) =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        serviceProvider.GetRequiredService<ILogger<CatalogClient>>()
                            .LogWarning($"Opening the circuit for {timeSpan.TotalSeconds} seconds...");
                    },
                    onReset: () =>
                    {
                        var serviceProvider = services.BuildServiceProvider();
                        serviceProvider.GetRequiredService<ILogger<CatalogClient>>()
                            .LogWarning("Closing the circuit...");
                    }))
            .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
    }
}