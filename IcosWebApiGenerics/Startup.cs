using IcosWebApiGenerics.ControllerFactory;
using IcosWebApiGenerics.Data;
using IcosWebApiGenerics.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using IcosWebApiGenerics.Models;


namespace IcosWebApiGenerics
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
            services.AddDbContext<IcosDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped(typeof(IValidatorService<>), typeof(ValidatorService<>));
            services.AddScoped(typeof(ISaveDataService<>), typeof(SaveDataService<>));
            services.AddScoped<IValidateService, ValidateService>();
            services.AddScoped<IMapper, Mapper>();
            services.AddScoped<IErrorLogger, ErrorLogger>();

            services.
                    AddMvc(o => o.Conventions.Add(
                        new GenericControllerRouteConvention()
                    )).
                    ConfigureApplicationPartManager(m =>
                        m.FeatureProviders.Add(new GenericTypeControllerFeatureProvider()
                    ));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
