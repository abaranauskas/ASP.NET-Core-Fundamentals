using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Routing;
using OdeToFood.Services;
using OdeToFood.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace OdeToFood
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                                  .SetBasePath(env.ContentRootPath)
                                  .AddJsonFile("appsettings.json")
                                  .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton(Configuration);
            //Asp naudos Configuration metoda visur kur reikes Iconfiguration
            //kaip tik Greeter reikalingas Iconfiguration in ctor
            //AddSingleton() cia bus vienas objectas visam app sukuriamas
            services.AddSingleton<IGreeter, Greeter>();

            //kai tik reikalinga kontrolery IRestaurantData
            //bus sukurtas InmemoryRestaurantDarta objectas
            //AddScope() cia bus sukuriamas naujas objektas po kiekvieno http requesto
            services.AddScoped<IRestaurantData, SqlRestaurantData>();
            
            //conection string paemimas is appsettings.json
            services.AddDbContext<OdeToFoodDbContext>(option => 
                    option.UseSqlServer(Configuration.GetConnectionString("OdeToFood")));

            services.AddIdentity<User, IdentityRole>()
                    .AddEntityFrameworkStores<OdeToFoodDbContext>();
        }

        // This method gets called by the runtime. Use this method to configure 
        //the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env,
            ILoggerFactory loggerFactory, 
            IGreeter greeter)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {

                //turi but virsuje nes jei bus paskutinis niekad
                //nesuveiks. jis callinamas o tada laukia ko kitos 
                //middleware dalys grazins exeptiona
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //sis kodas veiks viduose mode iskyrus development
                app.UseExceptionHandler(new ExceptionHandlerOptions()
                {
                    ExceptionHandler = context => context.Response.WriteAsync("Oop Beda!")
                });
            }
            
            app.UseFileServer();

            app.UseIdentity();
            app.UseNodeModules(env.ContentRootPath);
            app.UseMvc(ConfigureRoutes);

            //context lamdose trumpinamas kaip ctx
            //app.Run(ctx => ctx.Response.WriteAsync("Nerastas toks puslapis"));

            //----Viskas kas buvo bandoma iki MVC kai dirbama su MVC to nereikia-------
            //demonstacija kaip workflow ir middleware veikia 
            //IkiMVC(app, greeter);
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            // /Home/Index/
            //cia vadinamasis convention based routing
            routeBuilder.MapRoute("Default",
                "{controller=Home}/{action=Index}/{id?}");
        }

        private static void IkiMVC(IApplicationBuilder app, IGreeter greeter)
        {
            //app.UseDefaultFiles(); //
            //app.UseStaticFiles(); //jei be UsedDefaultFiles naudojmas
            // ledzia paimti static file is wwroot, jei UsedDefaultFiles
            //naudojama seka tada pavers ji defaut page

            app.UseFileServer();
            //darys ta pati ka prie tai esancios 2 eilutis kartu 


            //app.UseWelcomePage("/Welcome");
            //kita eilute darys visiskai tapati bus pasikiama tik per /welcomw
            app.UseWelcomePage(new WelcomePageOptions()
            {
                Path = "/welcome"
            });

            app.Run(async (context) =>
            {
                //throw new Exception("Kazkas nutiko!!!");
                var message = greeter.GetGreeting();

                await context.Response.WriteAsync(message);
            });
        }
    }
}
