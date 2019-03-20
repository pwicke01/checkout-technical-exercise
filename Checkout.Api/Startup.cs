using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Checkout.Data;
using Checkout.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

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
        x.SwaggerDoc("v1", new Info { Title = "Checkout API", Version = "v1" });
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
