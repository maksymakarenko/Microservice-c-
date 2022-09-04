namespace CommandsServ.EventProc
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}