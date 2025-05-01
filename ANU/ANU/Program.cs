using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Logging;
using ANU.Models;             // ⟵ your DbContext namespace

namespace ANU
{
    public class EntryPoint
    {
        public static void Main(string[] args)
        {
            // Build host
            var host = CreateHostBuilder(args).Build();

            // === APPLY MIGRATIONS AT STARTUP ===
            // Create a scope to grab your ApplicationDbContext
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    // Resolve your DbContext
                    var context = services.GetRequiredService<ApplicationDbContext>();

                    // Apply any pending migrations
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    // Log errors if migration fails
                    var logger = services.GetRequiredService<ILogger<EntryPoint>>();
                    logger.LogError(ex, "An error occurred applying migrations.");
                }
            }
            // =====================================

            // Run the web host
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
