using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using Npgsql.EntityFrameworkCore.PostgreSQL;
using ParkSquare.AspNetCore.Sitemap;

using Echo.Data;
using Echo.Hubs;
using Echo.Roles;
using Echo.Models;
using Echo.Hashers;
using Echo.Services;
using Echo.Interfaces;
using Echo.Middlewares;
using Echo.Repositories;

namespace Echo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

			var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
			app.UseRequestLocalization(options.Value);

            app.UseNotFoundMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // Default routes
                endpoints.MapControllers().RequireAuthorization();

                // Chat Hub
                endpoints.MapHub<ChatHub>("/chatHub");
            });

			app.UseSitemap();
        }

        public void ConfigureServices(IServiceCollection services)
        {
			services.AddSitemap();

			services.AddLogging(builder =>
			{
				builder.AddConsole();
			});

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
			{
				string defaultCulture = "en";
				var supportedCultures = new[] { defaultCulture, "ru" };
				var cultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList();
				options.DefaultRequestCulture = new RequestCulture(defaultCulture);
				options.SupportedCultures = cultures;
				options.SupportedUICultures = cultures;
			});

            services.Configure<RazorViewEngineOptions>(o =>
            {
                // {2} is area, {1} is controller,{0} is the action    
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Controllers/Shared/Views/{0}" + RazorViewEngine.ViewExtension);

                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add("/Areas/{2}/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Areas/{2}/Controllers/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Areas/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
				options.LoginPath = "/login";
				options.LogoutPath = "/logout";
				options.AccessDeniedPath = "/403";
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.SlidingExpiration = true;
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy("RequireAdminRole", policy =>
				{
					policy.RequireRole("Admin");
				});

				options.DefaultPolicy = new AuthorizationPolicyBuilder()
				  .RequireAuthenticatedUser()
				  .Build();
			});

			services.AddIdentity<User, UserRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultUI()
				.AddDefaultTokenProviders();

			services.AddDbContext<AppDbContext>(options =>
				options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

			services.AddHttpsRedirection(options =>
			{
				options.HttpsPort = 5001;
			});

			services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();

			services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization()
				.AddRazorRuntimeCompilation();

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = 102400000;
            });

			services.AddScoped<UserRepository>();

			services.AddScoped<RoleManager<UserRole>>();
			services.AddScoped<AppRoleManager>();

			services.AddScoped<UserService>();
			services.AddScoped<IUserServiceResult, UserServiceResult>();
			services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
			services.AddScoped<SignInManager<User>, SignInManager<User>>();
		}
    }
}
