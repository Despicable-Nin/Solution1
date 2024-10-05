using BlazorApp2.Components;
using BlazorApp2.Components.Account;
using BlazorApp2.Data;
using BlazorApp2.Helpers;
using BlazorApp2.Services.Geocoding;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace BlazorApp2;

public class Program
{

    public static void Main(string[] args)
    {
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug() // Set minimum log level
            .Enrich.FromLogContext()
            .WriteTo.Console() // Log to console (optional)
            .WriteTo.Seq("http://seq:5341") // Set Seq URL
            .CreateLogger();



        var builder = WebApplication.CreateBuilder(args);


        // Use Serilog for logging
        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

        builder.Services.AddMyServices();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
            .AddIdentityCookies();


        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException($"Connection string 'DefaultConnection' not found.");

        Console.WriteLine(connectionString);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

        builder.Services.AddMyRepositories();
        builder.Services.AddMyServices();
        builder.Services.AddHttpClient<NominatimGeocodingService>();


        builder.Services.AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo(@"/var/dpkeys"));  // Mount this volume in your docker-compose file

        var app = builder.Build();

        // Apply migrations automatically at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            try
            {
                dbContext.Database.Migrate();
                Console.WriteLine("Database migration applied successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during migration: {ex.Message}");
            }
        }


        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
            // Disable HTTPS redirection for development
            app.UseHttpsRedirection(); // Comment out this line
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();  // Keep this for production
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.Run();
    }
}
