using FerryToHelsinki.Data;
using FerryToHelsinki.Hubs;
using FerryToHelsinkiWebsite.Data.Models;
using FerryToHelsinkiWebsite.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace FerryToHelsinki
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<FerryToHelsinkiDBContext>(options => 
                options.UseSqlServer(
                    Configuration.GetConnectionString("FerryToHelsinkiContext")));
            services.AddRazorPages();
            services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
            services.AddSignalR(options => options.EnableDetailedErrors = true);
            services.AddScoped<MessageRepository>();
            services.AddScoped<MessageClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");

                endpoints.MapHub<MessageHub>("/messagehub");
            });
        }
    }
}
