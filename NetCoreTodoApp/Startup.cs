using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetCoreTodoApp.Business;
using NetCoreTodoApp.Business.Abstract;
using NetCoreTodoApp.Data;
using NetCoreTodoApp.Data.Abstract;
using NetCoreTodoApp.Filters;
using NetCoreTodoApp.Framework.Abstract.Services;
using NetCoreTodoApp.Framework.Services;
using NetCoreTodoApp.Model;

namespace NetCoreTodoApp
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
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(
					Configuration.GetConnectionString("DefaultConnection"),
					optionsBuilder => optionsBuilder.MigrationsAssembly("NetCoreTodoApp.Data")));

			services.AddDatabaseDeveloperPageExceptionFilter();

			services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();

			services.AddControllersWithViews(config => config.Filters.Add(typeof(TodoAppExceptionFilter)));

			SetContainerConfiguration(services);
		}

		private static void SetContainerConfiguration(IServiceCollection services)
		{
			services.AddScoped<ITodoService, TodoService>();
			services.AddScoped<ITodoRepository, TodoRepository>();
			services.AddScoped<IUserManagerWrapper, UserManagerWrapper>();

			services.AddSingleton<ITodoFactory, TodoFactory>();
			services.AddSingleton<ILoggingService, LoggingService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseMigrationsEndPoint();
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
		}
	}
}