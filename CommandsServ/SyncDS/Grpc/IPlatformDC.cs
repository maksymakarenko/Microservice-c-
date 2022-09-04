using CommandsServ.DataLayerModels;

namespace CommandsServ.SyncDS.Grpc
{
    public interface IPlatformDC
    {
        IEnumerable<Platform> ReturnPlatforms();
    }
}