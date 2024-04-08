namespace ParseIP.Services.Interfaces
{
    public interface ILogReader
    {
        IEnumerable<string> Read(string logFilePath);
    }
}
