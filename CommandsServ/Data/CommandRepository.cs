using CommandsServ.DataLayerModels;

namespace CommandsServ.Data
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public void CreateCommand(int platformId, Command command)
        {
           if(command == null)
           {
                throw new ArgumentNullException(nameof(command));
           }

           command.PlatformId = platformId;
           _context.Commands.Add(command);
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform == null)
            {
                throw new ArgumentNullException(nameof(platform));
            }

            _context.Platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.Commands.Where(p => p.PlatformId == platformId &&  p.Id == commandId)
                                    .FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsFromPlatforms(int platformId)
        {
            return _context.Commands.Where(p => p.PlatformId == platformId)
                                    .OrderBy(p => p.Platform.Name);
        }

        public bool platformExist(int platformId)
        {
            return _context.Platforms.Any(p => p.Id == platformId);
        }

        public bool saveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}