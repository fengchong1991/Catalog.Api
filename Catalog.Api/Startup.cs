using System;
using System.Data.Common;
using Catalog.Api.Infrastructure;
using Catalog.Api.IntegrationEvents;
using EventBus.Abstractions;
using IntegrationEventLog.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EventBusRabbitMQ;
using RabbitMQ.Client;
using EventBus;

namespace Catalog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            var connectionString = Configuration["ConnectionString"];
            services.AddDbContext<CatalogContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"]);
            });
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "eShopOnContainers - catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API"
                });
            });

            AddIntegrationServices(services);
            AddEventBus(services, Configuration);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger().UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void AddIntegrationServices(IServiceCollection services)
        {
            services.AddTransient<ICatalogIntegrationEventService, CatalogIntegrationEventService>();

            services.AddTransient<Func<DbConnection, IIntegrationEventLogService>>(sp => connection =>
            {
                return new IntegrationEventLogService(connection);
            });


        }
        
        private void AddEventBus(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>();
            services.AddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
            {
                return new ConnectionFactory()
                {
                    HostName = configuration["EventBusConnection"],
                    DispatchConsumersAsync = true
                };
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
