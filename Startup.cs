using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WCE.Models;

namespace WCE
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
            // services.AddMVC();
            //services.Add(new ServiceDescriptor(typeof(CountryStoreContext), new CountryStoreContext(Configuration.GetConnectionString("DefaultConnection"))));          
            services.Add(new ServiceDescriptor(typeof(LoginStoreContext), new LoginStoreContext(Configuration.GetConnectionString("DefaultConnection"))));
            services.Add(new ServiceDescriptor(typeof(StudentStoreContext), new StudentStoreContext(Configuration.GetConnectionString("DefaultConnection"))));          
            services.Add(new ServiceDescriptor(typeof(TeacherStoreContext), new TeacherStoreContext(Configuration.GetConnectionString("DefaultConnection"))));          
            services.Add(new ServiceDescriptor(typeof(TestStoreContext), new TestStoreContext(Configuration.GetConnectionString("DefaultConnection"))));          
            services.Add(new ServiceDescriptor(typeof(QuestionStoreContext), new QuestionStoreContext(Configuration.GetConnectionString("DefaultConnection"))));          
            services.Add(new ServiceDescriptor(typeof(AnswerStoreContext), new AnswerStoreContext(Configuration.GetConnectionString("DefaultConnection"))));          
            services.Add(new ServiceDescriptor(typeof(TestAttemptsStoreContext), new TestAttemptsStoreContext(Configuration.GetConnectionString("DefaultConnection"))));          
            services.AddControllersWithViews();
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
