using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Odata7Test.Models;

namespace Odata7Test
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
            services.AddMvc();
            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var builder = new ODataConventionModelBuilder(app.ApplicationServices);
            builder.EntitySet<Car>("Cars");
        
            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Count().Filter().OrderBy().Select().MaxTop(null).Expand();
                routeBuilder.MapODataServiceRoute("ODataRoute", "odata", builder.GetEdmModel());
                // Work-around for #1175

                routeBuilder.EnableDependencyInjection();
            } ) ;
        }
    }
}
