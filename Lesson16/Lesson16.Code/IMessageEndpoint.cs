namespace Lesson16.Code
{
    public interface IMessageEndpoint
    {
        IMessageHandlerResponse HandleMessage(string messageJson);
    }
}