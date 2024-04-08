
using ParseIP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace ParseIP.Services.Implementations
{
    public class LogAnalyzer : ILogAnalyzer
    {
        private readonly ILogParser _logParser;

        private readonly ILogReader _logFileReader;

        private readonly ILogWriter _logOutputWriter;

        public LogAnalyzer(ILogParser logParser, ILogReader logFileReader, ILogWriter logOutputWriter)
        {
            _logParser = logParser;

            _logFileReader = logFileReader;

            _logOutputWriter = logOutputWriter;
        }

        public void Analyze(string logFilePath, string outputFilePath, IPAddress addressStart, IPAddress addressMask, DateTime? timeStart, DateTime? timeEnd)
        {
            var logs = _logFileReader.Read(logFilePath);

            var ipCounts = _logParser.Parse(logs, addressStart, addressMask, timeStart, timeEnd);

            _logOutputWriter.Write(outputFilePath, ipCounts);
        }
    }
}
