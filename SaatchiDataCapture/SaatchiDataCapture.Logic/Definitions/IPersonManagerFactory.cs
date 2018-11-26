namespace SaatchiDataCapture.Logic.Definitions
{
    public interface IPersonManagerFactory
    {
        IPersonManager Create(ILoggerProvider loggerProvider);
    }
}
