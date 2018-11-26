namespace SaatchiDataCapture.Logic.Definitions
{
    public interface IPersonManagerSettingsProvider
    {
        string DataCaptureDatabaseConnectionString
        {
            get;
        }
    }
}