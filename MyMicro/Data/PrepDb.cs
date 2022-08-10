using System;
using Microsoft.EntityFrameworkCore;
namespace MyMicro.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isProd)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }

        private static void SeedData(AppDbContext context, bool isProd)
        {
            if(isProd)
            {
                Console.WriteLine("--> Attempting to apply migration");
                try
                {
                    context.Database.Migrate();
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"--> Cannot apply migration :{ex.Message}");
                }
            }

            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding ...");

                context.Platforms.AddRange(
                    new Models.Platform() {Name = "A", Publisher = "v", Cost = "free"},
                    new Models.Platform() {Name = "B", Publisher = "vi", Cost = "free"},
                    new Models.Platform() {Name = "C", Publisher = "vim", Cost = "free"}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> Data is already existing");
            }
        }
    }
}