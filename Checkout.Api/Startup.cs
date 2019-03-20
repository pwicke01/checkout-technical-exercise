using Checkout.Data;
using Checkout.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace Checkout.Api
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
      services.AddDbContext<CheckoutContext>(opt => opt.UseInMemoryDatabase("Checkout"));
      services.AddScoped<CustomerService>();
      services.AddScoped<BasketService>();
      services.AddScoped<ProductService>();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
        .AddJsonOptions(x => x.SerializerSettings.Formatting = Formatting.Indented)
        .AddJsonOptions(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

      services.AddSwaggerGen(x =>
      {
        x.SwaggerDoc("v1", new Info
          {
            Title = "Checkout API",
            Version = "v1",
            Description = "An API that allows customers to add products to a basket, remove them or empty the basket altogether.",
            Contact = new Contact
            {
              Email = "phflinckq@hotmail.com",
              Name = "Philip Wickens"
            }
          }
        );

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        x.IncludeXmlComments(xmlPath);
      });
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

      app.UseSwagger();

      app.UseSwaggerUI(x =>
      {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "Checkout API V1");
        x.RoutePrefix = string.Empty;
      });

      app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}
