using CommandsServ.DataLayerModels;
using CommandsServ.SyncDS.Grpc;

namespace CommandsServ.Data
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDC>();
                var platforms = grpcClient.ReturnPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
            }
        }

        private static void SeedData(ICommandRepository repository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> Seeding Platforms");

            foreach (var platform in platforms)
            {
                if(!repository.platformExist(platform.ExternalId))
                    repository.CreatePlatform(platform);

                repository.saveChanges();
            }
        }
    }
}