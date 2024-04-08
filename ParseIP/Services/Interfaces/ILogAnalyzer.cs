using System.Net;

namespace ParseIP.Services.Interfaces
{
    public interface ILogAnalyzer
    {
        void Analyze(string logFilePath, string outputFilePath, IPAddress addressStart, IPAddress addressMask, DateTime? timeStart, DateTime? timeEnd);
    }
}
