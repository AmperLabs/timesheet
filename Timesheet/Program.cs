using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FluentUI.AspNetCore.Components;
using System;
using System.Reflection;
using Timesheet.Components;
using Timesheet.Endpoints;
using Timesheet.Services;

namespace Timesheet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddFluentUIComponents();

            builder.Services.AddAuth0WebAppAuthentication(options =>
            {
                options.Domain = builder.Configuration["Auth0:Domain"];
                options.ClientId = builder.Configuration["Auth0:ClientId"];
                options.Scope = "openid profile email";
            });

            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<CalendarService>();
            builder.Services.AddScoped<BunkaiService>();
            builder.Services.AddSingleton<PdfRendererService>();
            builder.Services.AddSingleton<BlobService>();

            builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.MapEndpoints();

            app.MapGet("/login", async (HttpContext httpContext, string returnUrl = "/") =>
            {
                var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                    .WithRedirectUri(returnUrl)
                    .Build();

                authenticationProperties.IsPersistent = true;

                await httpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            });

            //app.MapPost("/logout", async (HttpContext httpContext) =>
            app.MapGet("/logout", async (HttpContext httpContext) =>
            {
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be added to the
                // **Allowed Logout URLs** settings for the app.
                var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                    .WithRedirectUri("/")
                    .Build();

                await httpContext.SignOutAsync(Auth0Constants.AuthenticationScheme);
                await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            });

            app.Run();
        }
    }
}
