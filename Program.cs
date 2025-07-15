using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DepoProjesi.Data;

namespace DepoProjesi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession();

            //
            builder.Services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", options =>
                {
                    options.Cookie.Name = "CookieAuth";
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Shared/AccessDenied";
                });

            // ➤ Veritabanı bağlantısı
            builder.Services.AddDbContext<WarehouseDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DepoDb"))
            );

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            // Razor controller route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            //  Web API Controller'lar için gerekli
            app.MapControllers();

            app.Run();

        }
    }
}
