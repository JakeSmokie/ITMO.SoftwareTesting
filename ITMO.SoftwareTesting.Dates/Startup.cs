using System.Linq;
using System.Security.Claims;
using System.Text;
using ITMO.SoftwareTesting.Dates.Contracts.Abstracts;
using ITMO.SoftwareTesting.Dates.Database.Abstracts;
using ITMO.SoftwareTesting.Dates.Services.Services;
using ITMO.SoftwareTesting.Datings.Database;
using ITMO.SoftwareTesting.Datings.Filters;
using ITMO.SoftwareTesting.Datings.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace ITMO.SoftwareTesting.Datings
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add(typeof(HttpGlobalExceptionFilter)));
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "itmo.softwaretesting.datings/dist";
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = "https://localhost:5000";
                options.Authority = null;
                options.Audience = "Users";
                options.SaveToken = false;

                options.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes("CompleteKickAssOmgThatIsSoBad"));

                options.TokenValidationParameters.RequireAudience = false;
                options.TokenValidationParameters.RequireSignedTokens = false;
                options.TokenValidationParameters.ValidateLifetime = false;
                options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                options.TokenValidationParameters.ValidateIssuer = false;
                options.TokenValidationParameters.ValidateAudience = false;
                options.TokenValidationParameters.SaveSigninToken = false;
                options.TokenValidationParameters.ValidateTokenReplay = false;
                options.TokenValidationParameters.RequireExpirationTime = false;
                options.TokenValidationParameters.RequireSignedTokens = false;
            });

            services.AddAuthorization();
            services.AddHttpContextAccessor();

            services.AddScoped<IUserContext>(r =>
            {
                var claimsPrincipal = r.GetService<IHttpContextAccessor>().HttpContext.User;
                var claims = claimsPrincipal.Claims.ToList();
                var userId = claims.SingleOrDefault(x => x.Type == ClaimsIdentity.DefaultNameClaimType)?.Value ?? "0";

                return new UserContext(int.Parse(userId));
            });

            services.AddSingleton<IDbContextFactory, DbContextFactory>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "itmo.softwaretesting.datings";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
                }
            });
        }
    }
}