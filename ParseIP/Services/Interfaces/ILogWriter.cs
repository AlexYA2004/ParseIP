using System.Net;

namespace ParseIP.Services.Interfaces
{
    public interface ILogWriter
    {
        void Write(string outputFilePath, Dictionary<IPAddress, int> ipCounts);
    }
}
