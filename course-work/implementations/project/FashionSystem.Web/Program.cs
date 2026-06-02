using FashionSystem.Web.Services;

namespace FashionSystem.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            // Session
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout =
                    TimeSpan.FromHours(2);

                options.Cookie.HttpOnly = true;

                options.Cookie.IsEssential = true;
            });

            // HttpContext
            builder.Services.AddHttpContextAccessor();

            // API Client
            builder.Services.AddHttpClient(
                "Api",
                client =>
                {
                    client.BaseAddress =
                        new Uri("https://localhost:7182/");
                });

            // Services
            builder.Services.AddScoped<AuthService>();

            builder.Services.AddScoped<FashionItemService>();

            builder.Services.AddScoped<RentalService>();

            builder.Services.AddScoped<UserService>();

            var app = builder.Build();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // IMPORTANT
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            // Default Route
            app.MapControllerRoute(
                name: "default",
                pattern:
                    "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}