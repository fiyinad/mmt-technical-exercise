using System.Reflection;
using Customers.Clients;
using Customers.Database;
using Customers.Database.Repository;
using Customers.Extensions;
using Customers.HealthCheck.Helpers;
using Customers.Models.Requests;
using Customers.Services;
using Customers.Settings;
using Customers.Swagger;
using Customers.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Customers
{
  public class Startup
  {
    /// <summary>
    /// Creates an instance of <see cref="Startup"/>
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddRouting(options => options.LowercaseUrls = true);

      services.AddControllers();

      services.AddVersionedApiExplorer(c =>
      {
        c.GroupNameFormat = "'v'V";
        c.SubstituteApiVersionInUrl = true;
      });
      services.AddSwaggerGen();
      services.AddApiVersioning();
      services.AddTransient<ISwaggerProvider, SwaggerGenerator>();
      services.AddTransient<ISchemaGenerator, SchemaGenerator>();
      services.AddTransient<IConfigureOptions<SwaggerGenOptions>, MySwaggerGenOptions>();

      // settings
      services.Configure<DbSettings>(Configuration.GetSection(nameof(DbSettings)));
      services.Configure<CustomerClientSettings>(Configuration.GetSection(nameof(CustomerClientSettings)));

      // DB
      var dbSettings = Configuration.GetSection(nameof(DbSettings)).Get<DbSettings>();
      services.AddDbContextPool<OrdersDbContext>(options => options.UseSqlServer(dbSettings.ConnectionString));

      // repositor(ies)
      services.AddTransient<IOrdersRepository, OrdersRepository>();

      // validators
      services.AddTransient<IValidator<CustomerOrderRequest>, CustomerOrderRequestValidator>();

      // mapper(s)
      services.AddAutoMapper(Assembly.GetExecutingAssembly());

      // client(s)
      services.AddHttpClient<ICustomerClient, CustomerClient>();

      // service(s)
      services.AddTransient<ICustomersService, CustomersService>();

      // health checks
      services.AddHealthChecks()
        .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "self" });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(
      IApplicationBuilder app, 
      IWebHostEnvironment env,
      IApiVersionDescriptionProvider provider)
    {
      if (env.IsLocal() || env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseApiVersioning();
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        foreach (var description in provider.ApiVersionDescriptions)
        {
          c.SwaggerEndpoint(
              $"/swagger/{description.GroupName}/swagger.json",
              $"{description.GroupName.ToUpperInvariant()}");
        }
      });

      app.UseHealthChecks("/healthz/self", HealthCheckHelpers.FormattedOptions(new[] { "self" }));

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
