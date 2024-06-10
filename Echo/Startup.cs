using System.Text;
using System.Globalization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using ParkSquare.AspNetCore.Sitemap;

using Echo.Data;
using Echo.Hubs;
using Echo.Roles;
using Echo.Models;
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

        
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddSitemap();

			services.AddLogging(builder =>
			{
				builder.AddConsole();
			});

			TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
			services.AddSingleton(timeZone);

            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
			{
				string defaultCulture = "en";

				var supportedCultures = new[] { defaultCulture, "ru", "ro" };
				var cultures = supportedCultures.Select(culture => new CultureInfo(culture)).ToList();

				options.DefaultRequestCulture = new RequestCulture(defaultCulture);
				options.SupportedCultures = cultures;
				options.SupportedUICultures = cultures;

				options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
			});

            services.Configure<RazorViewEngineOptions>(o =>
            {
                // {2} is area, {1} is controller,{0} is the action    
                o.ViewLocationFormats.Clear();
                o.ViewLocationFormats.Add("/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Controllers/Shared/{0}" + RazorViewEngine.ViewExtension);
                o.ViewLocationFormats.Add("/Controllers/Shared/Views/{0}" + RazorViewEngine.ViewExtension);

                o.AreaViewLocationFormats.Clear();
                o.AreaViewLocationFormats.Add("/Areas/{2}/Controllers/{1}/Views/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Areas/{2}/Controllers/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
                o.AreaViewLocationFormats.Add("/Areas/Shared/Views/{0}" + RazorViewEngine.ViewExtension);
            });

			var jwtSettings = Configuration.GetSection("Jwt");
			var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwtSettings["Issuer"],
					ValidAudience = jwtSettings["Audience"],
					IssuerSigningKey = new SymmetricSecurityKey(key)
				};
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

				options.FallbackPolicy = new AuthorizationPolicyBuilder()
					.RequireAuthenticatedUser()
					.Build();
			});

			services.AddIdentity<User, UserRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
				options.Password.RequiredUniqueChars = 1;

				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				options.Lockout.MaxFailedAccessAttempts = 5;
				options.Lockout.AllowedForNewUsers = true;

				options.User.AllowedUserNameCharacters =
				"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultUI()
				.AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.LoginPath = "/login";
				options.LogoutPath = "/logout";
				options.AccessDeniedPath = "/403";
				options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
				options.SlidingExpiration = true;
			});

			services.AddDbContext<AppDbContext>(options =>
				options
					.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
						o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

			services.AddHttpsRedirection(options =>
			{
				options.HttpsPort = 5001;
			});

			services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
			services.AddSingleton<IUserConnectionManager, UserConnectionManager>();

			services.AddMvc()
				.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
				.AddDataAnnotationsLocalization()
				.AddRazorRuntimeCompilation()
				.AddNewtonsoftJson(options =>
					options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
				);

			services.AddAntiforgery(options => 
			{
				options.HeaderName = "X-CSRF-TOKEN";
			});

			services.AddAntiforgery(options =>
			{
				options.FormFieldName = "AntiforgeryFieldname";
				options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
				options.SuppressXFrameOptionsHeader = false;
			});

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.MaximumReceiveMessageSize = 102400000;
            });

			services.AddScoped<UserRepository>();
			services.AddScoped<ChatRepository>();

			services.AddScoped<RoleManager<UserRole>>();
			services.AddScoped<AppRoleManager>();

			services.AddScoped<UserService>();
			services.AddScoped<ChatService>();
			services.AddScoped<FriendshipService>();
			services.AddScoped<IServiceResult<List<User>>, ServiceResult<List<User>>>();
			services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
			services.AddScoped<SignInManager<User>, SignInManager<User>>();
		}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<RequestLocalizationOptions> localizationOptions)
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

			app.Use(async (context, next) =>
			{
				context.Response.Headers.Append("Content-Security-Policy",
					"default-src 'self'; " +
					"script-src 'self' 'unsafe-inline' 'unsafe-eval' https://cdnjs.cloudflare.com; " + 
					"style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.jsdelivr.net https://unicons.iconscout.com; " + 
					"img-src 'self' blob: https://upload.wikimedia.org ; " + 
					"font-src 'self' https://fonts.gstatic.com https://unicons.iconscout.com; " + 
					"connect-src 'self' https://api.cdnjs.com; " +
					"upgrade-insecure-requests; " +
					"block-all-mixed-content");
				await next();
			});

            app.UseNotFoundMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
			{
				ContentTypeProvider = new FileExtensionContentTypeProvider
				{
					Mappings = { [".js"] = "application/javascript" }
				}
			});

            app.UseRouting();

            app.UseRequestLocalization(localizationOptions.Value);

			app.UseAntiforgery();

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
    }
}
