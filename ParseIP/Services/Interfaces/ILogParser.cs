using System.Net;

namespace ParseIP.Services.Interfaces
{
    public interface ILogParser
    {
        Dictionary<IPAddress, int> Parse(IEnumerable<string> logs, IPAddress? addressStart, IPAddress? addressMask, DateTime? timeStart, DateTime? timeEnd);
    }
}
