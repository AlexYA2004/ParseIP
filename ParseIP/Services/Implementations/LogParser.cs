using ParseIP.Services.Interfaces;
using System.Net;

namespace ParseIP.Services.Implementations
{
    public class LogParser : ILogParser
    {
        public Dictionary<IPAddress, int> Parse(IEnumerable<string> logs, IPAddress? addressStart, IPAddress? addressMask, DateTime? timeStart, DateTime? timeEnd)
        {
            var ipCounts = new Dictionary<IPAddress, int>();

            foreach (var log in logs)
            {
                var parts = log.Split(' ');

                if (parts.Length >= 2)
                {
                    if (IPAddress.TryParse(parts[0], out IPAddress ipAddress))
                    {
                        if (DateTime.TryParse(parts[1], out DateTime requestTime))
                        {
                            if ((!timeStart.HasValue || requestTime >= timeStart) && (!timeEnd.HasValue || requestTime <= timeEnd))
                            {
                                if (addressStart == null || addressMask == null || IsAddressInRange(ipAddress, addressStart, addressMask))
                                {
                                    if (ipCounts.ContainsKey(ipAddress))
                                        ipCounts[ipAddress]++;
                                    else
                                        ipCounts[ipAddress] = 1;
                                }
                            }
                        }
                    }
                }
            }

            return ipCounts;
        }

        private bool IsAddressInRange(IPAddress ipAddress, IPAddress addressStart, IPAddress addressMask)
        {
            byte[] addressBytes = ipAddress.GetAddressBytes();

            byte[] startBytes = addressStart.GetAddressBytes();

            byte[] maskBytes = addressMask.GetAddressBytes();

            for (int i = 0; i < addressBytes.Length; i++)
            {
                if ((addressBytes[i] & maskBytes[i]) != (startBytes[i] & maskBytes[i]))
                    return false;
            }

            return true;
        }
    }
}
