
namespace Assets.Scripts.Infrastructure.Logging
{
    internal interface ILogger
    {
        void Log(LogType type, object message, string messageTag="");
    }
}
