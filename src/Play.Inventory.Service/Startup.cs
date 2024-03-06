using Microsoft.OpenApi.Models;

using Play.Common.Identity;
using Play.Common.MassTransit;
using Play.Common.MongoDb;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service;

public sealed class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration => configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddMongo()
            .AddMongoRepository<InventoryItem>("inventoryItems")
            .AddMongoRepository<CatalogItem>("catalogItems")
            .AddMassTransitWithRabbitMq()
            .AddJwtBearerAuthentication();

        services.AddControllers(options =>
        {
            options.SuppressAsyncSuffixInActionNames = false;
        });

        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "Play.Inventory.Service", Version = "v1" }
            );
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Play.Inventory.Service v1")
            );

            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration["AllowedOrigin"] ?? string.Empty)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
