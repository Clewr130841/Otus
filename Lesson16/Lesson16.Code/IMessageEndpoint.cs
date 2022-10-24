namespace Lesson16.Code
{
    public interface IMessageEndpoint
    {
        void HandleMessage(string messageJson);
    }
}