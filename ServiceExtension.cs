using IdentityNetCore.Data;
using IdentityNetCore.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspIndentity_zajecia
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connString = configuration["ConnectionStrings:Default"];
            services.AddDbContext<ApplicationDBContext>(o => o.UseMySql(connString,
                new MySqlServerVersion(new Version(8, 0, 13))));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDBContext>();

            services.Configure<IdentityOptions>(options => {

                options.Password.RequiredLength = 3;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;

                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);

                options.SignIn.RequireConfirmedEmail = false;

            });

            services.ConfigureApplicationCookie(option => {
                option.LoginPath = "/Identity/Signin";
                option.AccessDeniedPath = "/Identity/AccessDenied";
                option.ExpireTimeSpan = TimeSpan.FromHours(10);
            });

            services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));

            services.AddSingleton<IEmailSender, SmtpEmailSender>();
            services.AddAuthorization(option => {

                option.AddPolicy("MemberDep", p => {

                    p.RequireClaim("Department", "Tech").RequireRole("Member");
                });

                option.AddPolicy("AdminDep", p => {

                    p.RequireClaim("Department", "Tech").RequireRole("Admin");
                });
            });
            services.AddControllersWithViews();

            return services;
        }
    }
}
