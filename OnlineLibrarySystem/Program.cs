using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using OnlineLibrarySystem.Data;
using OnlineLibrarySystem.Models;

namespace OnlineLibrarySystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register the ISendOtpMessage service
            builder.Services.AddTransient<ISendOtpMessage, SendOtpMessage>();

            // Register the ApplicationDbContext with the connection string
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Login/Login";
                options.LogoutPath = "/Logout/Logout";
                options.AccessDeniedPath = "/Login/AccessDenied";
            });
            builder.Services.AddAuthorization(optins =>
            {
                optins.AddPolicy("Admin",optins =>
                {
                    optins.RequireRole("Admin");
                });
                optins.AddPolicy("User", optins =>
                {
                    optins.RequireRole("User");
                });
            });
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
