using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Hcb.Insights
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
//            _configuration = configuration;
            Configuration = configuration;
        }

//        public IConfiguration _configuration { get; }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddResponseCompression();

            services.AddSession( options => 
            { 
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });
            services.AddMemoryCache();
//            services.AddDistributedMemoryCache();

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddHsts(options =>
            {
                options.IncludeSubDomains = true;
                //                options.MaxAge = new TimeSpan(0, 3, 0);
            });

            services.Configure<IISServerOptions>(options =>
            {   //Need this so that we can call synchronous methods such as Stream.ReadLine().
                options.AllowSynchronousIO = true;
            });
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

            app.UseRewriter(new RewriteOptions()
                .Add(new RedirectAzureWebsitesRule())
                .AddRedirectToWwwPermanent());
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    // Cache static file for 1 year
                    if (!string.IsNullOrEmpty(context.Context.Request.Query["v"]))
                    {
                        context.Context.Response.Headers.Add("cache-control", new[] { "public,max-age=31536000" });
                        context.Context.Response.Headers.Add("Expires", new[] { DateTime.UtcNow.AddYears(1).ToString("R") }); // Format RFC1123
                    }
                }
            });
            app.UseCookiePolicy();
            app.UseSession();   //Must be called after UseCookiePolicy
//            app.UseMvc();   //Must be called after UseSession.
            app.UseResponseCompression();   //Always call before UseMvc
            app.UseRouting();
            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapRazorPages();
                endpoints.MapControllers(); 
            });

            Environment.SetEnvironmentVariable("_RunningUnderMsTest", "false");
            string secret = Configuration.GetSection("hcb").GetValue<string>("reCaptchaSecret");
            Environment.SetEnvironmentVariable("hcb:reCaptchaSecret", secret);

        }
    }

    public class RedirectAzureWebsitesRule : IRule
    {
        public void ApplyRule(RewriteContext context)
        {
            HostString host = context.HttpContext.Request.Host;

            if (host.HasValue && host.Value.ToLower().Contains(".azurewebsites.net"))
            {
                //if we are viewing on azure's domain - skip the www redirect
                context.Result = RuleResult.SkipRemainingRules;
            }
            else
            {
                context.Result = RuleResult.ContinueRules;
            }
        }
    }
}
