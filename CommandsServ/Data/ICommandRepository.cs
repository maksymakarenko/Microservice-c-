using CommandsServ.DataLayerModels;

namespace CommandsServ.Data
{
    public interface ICommandRepository
    {
        bool saveChanges();

        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform platform);
        bool platformExist(int platformId);

        IEnumerable<Command> GetCommandsFromPlatforms(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}