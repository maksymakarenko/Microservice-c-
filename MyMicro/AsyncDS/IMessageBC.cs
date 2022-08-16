using MyMicro.Dtos;

namespace MyMicro.AsyncDS
{
    public interface IMessageBC
    {
        void PublishNewPlat(PlatformPublishedDto platform);
    }
}