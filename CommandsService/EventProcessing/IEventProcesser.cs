namespace CommandsService.EventProcessing
{
    public interface IEventProcesser
    {
        void ProcessEvent(string message);
    }
}
