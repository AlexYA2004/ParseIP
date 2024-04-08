using Microsoft.Extensions.DependencyInjection;
using ParseIP.Services.Implementations;
using ParseIP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseIP.Services
{
    public static class Initializer
    {
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<ILogWriter, LogWriter>();

            services.AddScoped<ILogReader, LogReader>();

            services.AddScoped<ILogParser, LogParser>();

            services.AddScoped<ILogAnalyzer, LogAnalyzer>();
        }
    }
}
