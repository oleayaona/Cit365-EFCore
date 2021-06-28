using ContosoUniversity.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ContosoUniversity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}


// Scaffold a controller in CLI
//dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
//dotnet add package Microsoft.EntityFrameworkCore.Design
//dotnet tool install --global dotnet-aspnet-codegenerator

// dotnet aspnet-codegenerator controller -name StudentsController -actions -m Student -dc SchoolContext -outDir Controllers

// dotnet-aspnet-codegenerator controller -name StudentsController -m Student -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

// dotnet-aspnet-codegenerator controller -name CoursesController -m Course -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

// dotnet-aspnet-codegenerator controller -name InstructorsController -m Instructor -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

// dotnet-aspnet-codegenerator controller -name DepartmentsController -m Department -dc SchoolContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
