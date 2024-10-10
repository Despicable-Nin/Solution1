using BlazorApp2.BackgroundServices;
using BlazorApp2.Components;
using BlazorApp2.Components.Account;
using BlazorApp2.Data;
using BlazorApp2.Helpers;
using BlazorApp2.Services.Geocoding;
using Blazored.Toast;
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
            .MinimumLevel.Information() // Set minimum log level
            .Enrich.FromLogContext()
            .WriteTo.Console() // Log to console (optional)
            .WriteTo.Seq("http://seq:5341") // Set Seq URL
            .CreateLogger();



        var builder = WebApplication.CreateBuilder(args);


        // Use Serilog for logging
        builder.Host.UseSerilog();
        builder.Services.AddBlazoredToast();


        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();


        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException($"Connection string 'DefaultConnection' not found.");

        Console.WriteLine(connectionString);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
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
        ApplyMigrationAndSeedDatabase(app);


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

    private static void ApplyMigrationAndSeedDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        try
        {
            dbContext.Database.Migrate();
            Console.WriteLine("Database migration applied successfully.");

            // Check for existing admin user
            var adminUser = userManager.FindByNameAsync("admin").Result;
            if (adminUser == null)
            {
                // Create admin user with desired password
                var newAdminUser = new ApplicationUser { UserName = "admin@io.com", Email = "admin@io.com" };
                newAdminUser.EmailConfirmed = true;

                // Replace "password" with a strong password
                var createResult = userManager.CreateAsync(newAdminUser, "P@ssword1234!").Result;

                if (createResult.Succeeded)
                {
                    // Assign admin role (if applicable)
                    var adminRole = roleManager.FindByNameAsync("Administrators").Result; // 
                    if (adminRole == null)
                    {
                        adminRole = new IdentityRole("Administrators");
                        roleManager.CreateAsync(adminRole).ConfigureAwait(false);
                    }
                    userManager.AddToRoleAsync(newAdminUser, adminRole.Name).ConfigureAwait(false);
                    Console.WriteLine("Admin user created successfully.");
                }
                else
                {
                    Console.WriteLine($"Error creating admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine("Admin user already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during migration or user creation: {ex.Message}");
            Log.Error(ex, "Error during migration or user creation"); // Log exception for debugging
        }
    }
}
