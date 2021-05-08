using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Assignment.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Assignment.DataAccess.Interfaces;
using Assignment.DataAccess.Repositories;
using Google.Cloud.SecretManager.V1;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Assignment
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment host)
        {
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", host.ContentRootPath +@"\pristine-abacus-307313-5bc44a544508.json");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionString();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));


            services.AddScoped<ICarsRepository, CarsRepository>();

            services.AddScoped<ICachingService, CachingService>();

            services.AddScoped<IBookingRepository, BookingRepository>();


            services.AddScoped<IPubSubRepository, PubSubRepository>();

            services.AddScoped<ILogRepository, LogRepository>();

            services.AddScoped<IEmailRepository, EmailRepository>();


            //       services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //           .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                        .AddDefaultUI()
                        .AddEntityFrameworkStores<ApplicationDbContext>()
                        .AddDefaultTokenProviders();
            services.AddControllersWithViews();
            services.AddRazorPages();

            string clientId = GetGoogleClientId();
            string clientSecret = GetGoogleClientSecret();
            


            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

          

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));
                endpoints.MapPost("/Identity/Account/Register", context => Task.Factory.StartNew(() => context.Response.Redirect("/Identity/Account/Login", true, true)));

            });
        }



        //connection string
        //oauth2.0 id + secret
        //mailgun
        public string GetGoogleClientId()
        {
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            SecretVersionName secretVersionName = new SecretVersionName("pristine-abacus-307313", "ClientCredentials", "1");

            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            string payload = result.Payload.Data.ToStringUtf8();

            JsonConvert.DeserializeObject(payload);

            JObject jsonObject = JObject.Parse(payload);
            JToken jsonKey = jsonObject["Authentication:Google:ClientId"];
            
            return jsonKey.ToString();


        }

        public string GetGoogleClientSecret()
        {
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            SecretVersionName secretVersionName = new SecretVersionName("pristine-abacus-307313", "ClientCredentials", "1");

            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            string payload = result.Payload.Data.ToStringUtf8();

            JsonConvert.DeserializeObject(payload);

            JObject jsonObject = JObject.Parse(payload);
            JToken jsonKey = jsonObject["Authentication:Google:ClientSecret"];

            return jsonKey.ToString();


        }

        public string GetConnectionString()
        {
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            SecretVersionName secretVersionName = new SecretVersionName("pristine-abacus-307313", "ClientCredentials", "1");

            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            string payload = result.Payload.Data.ToStringUtf8();

            JsonConvert.DeserializeObject(payload);

            JObject jsonObject = JObject.Parse(payload);
            JToken jsonKey = jsonObject["DefaultConnection"];

            return jsonKey.ToString();

        }

        public string GetMailGunApiKey()
        {
            SecretManagerServiceClient client = SecretManagerServiceClient.Create();

            SecretVersionName secretVersionName = new SecretVersionName("pristine-abacus-307313", "ClientCredentials", "1");

            AccessSecretVersionResponse result = client.AccessSecretVersion(secretVersionName);

            string payload = result.Payload.Data.ToStringUtf8();

            JsonConvert.DeserializeObject(payload);

            JObject jsonObject = JObject.Parse(payload);
            JToken jsonKey = jsonObject["MailGun"];

            return jsonKey.ToString();

        }
    }
}