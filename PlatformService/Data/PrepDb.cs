using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace PlatformService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using(var serviceScope=app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(),isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if (isProd)
            {
                Console.WriteLine("--> attempting to migration");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {

                    Console.WriteLine($"--> Couldn't run migration:{ex.Message}");
                }
            }
            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Platforms.AddRange(
                    new Models.Platform() { Name="Dot Net",Publisher="Microsoft",Cost="Free"},
                    new Models.Platform() { Name = "SqlServer Express", Publisher = "Microsoft", Cost = "Free" },
                    new Models.Platform() { Name = "Kubernetes", Publisher = "Cloud Computing Foundation", Cost = "Free" }

                    );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We Already have data");
            }

        }
    }
}
