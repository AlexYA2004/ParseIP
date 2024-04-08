using ParseIP.Services.Interfaces;
using System.Net;

namespace ParseIP.Services.Implementations
{
    public class LogWriter : ILogWriter
    {
        public void Write(string outputFilePath, Dictionary<IPAddress, int> ipCounts)
        {
            using (var writer = new StreamWriter(outputFilePath, true))
            {
                foreach (var pair in ipCounts)
                {
                    writer.WriteLine($"{pair.Key}\t{pair.Value}");
                }
            }
        }
    }
}
