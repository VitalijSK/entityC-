using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using lab2Vitaliu.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace lab2Vitaliu
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args);


            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var userManager = services.GetRequiredService<UserManager<User>>();
                    var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    RoleInitializer.InitializeAsync(userManager, rolesManager).Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run(); 
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
                    WebHost.CreateDefaultBuilder(args)
                        .UseStartup<Startup>()
                        .ConfigureLogging(logging =>
                        {
                            logging.AddConsole();
                            logging.AddDebug();
                            logging.AddEventSourceLogger();
                        })
                        .UseKestrel(options =>
                            {
                                options.Limits.MaxConcurrentConnections = 100;
                                options.Limits.MaxRequestBodySize = 11 * 1024;
                                options.Limits.MinRequestBodyDataRate = new Microsoft.AspNetCore.Server.Kestrel.Core.MinDataRate(
                                    bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(11));
                                options.Limits.MinResponseDataRate = new Microsoft.AspNetCore.Server.Kestrel.Core.MinDataRate(
                                    bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(11));
                                options.Listen(IPAddress.Loopback, 50010);
                            }).Build();
    }
}
