using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Project.Entity;

namespace Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomerOrderContext>(options =>
                  options.UseSqlServer("Server=192.168.1.152;Database=odata-issue2;User Id=keatkeat;Password=001001;"));

            //services.AddControllers(mvcOptions =>
            //   mvcOptions.EnableEndpointRouting = false);

            //services.AddRazorPages();

            services.AddMvc()
                //.AddNewtonsoftJson()
                .AddMvcOptions(options =>
                {
                    options.EnableEndpointRouting = false;
                }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddOData();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                routeBuilder.EnableDependencyInjection();
                IEdmModel model = EdmModelBuilder.GetEdmModel();
                routeBuilder.MapODataServiceRoute("odata", "odata", model);
            });
        }

        public static class EdmModelBuilder
        {
            private static IEdmModel _edmModel;

            public static IEdmModel GetEdmModel()
            {
                if (_edmModel == null)
                {
                    var builder = new ODataConventionModelBuilder();
                    builder.EntitySet<Product>("Products");
                    _edmModel = builder.GetEdmModel();
                }

                return _edmModel;
            }
        }
    }
}
