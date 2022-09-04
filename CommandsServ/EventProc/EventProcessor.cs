using System.Text.Json;
using AutoMapper;
using CommandsServ.Data;
using CommandsServ.DataLayerModels;
using CommandsServ.Dtos;

namespace CommandsServ.EventProc
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var typeEvent = DeterminEvent(message);

            if(typeEvent == TypeOfEvent.PlatformIsPublished)
                addPlatform(message);
        }

        private TypeOfEvent DeterminEvent(string message)
        {
            Console.WriteLine("--> Determine Event");

            var typeEvent = JsonSerializer.Deserialize<GenericEventDto>(message);

            if(typeEvent.Event == "PlatformISPublished")
            {
                Console.WriteLine("--> Platform Publishe Detected");
                return TypeOfEvent.PlatformIsPublished;
            }
            else
            {
                Console.WriteLine("--> Couldn't Determine");
                return TypeOfEvent.Undetermined;
            }
        }

        private void addPlatform(string message)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);

                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);

                    if(!repository.platformExist(platform.ExternalId))
                    {
                        repository.CreatePlatform(platform);
                        repository.saveChanges();
                        Console.WriteLine("--> Platform Is Added");
                    }
                    else
                        Console.WriteLine("--> Platform Already Exist");
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine($"--> Couldn't Add Platform To DataBase {ex.Message}");
                }
            }
        }
    }

    enum TypeOfEvent
    {
        PlatformIsPublished,
        Undetermined
    }
}