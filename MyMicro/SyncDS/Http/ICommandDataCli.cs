using MyMicro.Dtos;

namespace MyMicro.SyncDS.Http
{
    public interface ICommandDataCli
    {
        Task SendPlatformToCommand(PlatformReadDto platform);
    }
}