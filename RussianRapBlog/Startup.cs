using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RussianRapBlog.Extensions;

namespace RussianRapBlog
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true);

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfigured();
            services.AddControllers();
            services.AddMvc();
            services.AddSwaggerConfigured();
            services.AddDatabase(_configuration);
            services.AddAuthenticationConfigured(_configuration);
            services.AddServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(o => o.SwaggerEndpoint("/swagger/v1/swagger.json", "RussianRapBlog Api"));
            }

            app.UseRouting();
            app.UseExceptionHandlerMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}