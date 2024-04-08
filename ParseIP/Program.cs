using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using ParseIP.Services;
using ParseIP.Services.Implementations;
using ParseIP.Services.Interfaces;
using System.Net;

namespace ParseIP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string logFilePath;

            string outputFilePath;

            IPAddress addressStart;

            IPAddress addressMask;

            DateTime timeStart;

            DateTime timeEnd;

      
            if (args.Length > 0)
            {
                ParseCommands(args, out logFilePath, out outputFilePath, out addressStart, out addressMask, out timeStart, out timeEnd);
            }
            else
            {
                Console.WriteLine("Аргументы не были введены.");

                Console.WriteLine("Желаете использовать файлы конфигурации? (Y/N)");

                string response = Console.ReadLine();

                if (response.Trim().ToUpper() == "Y")
                {
                    var builder = new ConfigurationBuilder();

                    builder.AddJsonFile("D:/Microsoft VS/Projects/ParseIP/ParseIP/appsettings.json", optional: false, reloadOnChange: true);

                    IConfigurationRoot configuration = builder.Build();

                    logFilePath = configuration.GetSection("DefaultLogFile").Value;

                    outputFilePath = configuration.GetSection("DefaultOutputFile").Value;

                    addressStart = IPAddress.Parse(configuration.GetSection("DefaultAddressStart").Value);

                    addressMask = ConvertDecimalToIP(int.Parse(configuration.GetSection("DefaultAddressMask").Value));

                    timeStart = DateTime.Parse(configuration.GetSection("DefaultTimeStart").Value);

                    timeEnd = DateTime.Parse(configuration.GetSection("DefaultTimeEnd").Value);

                }
                else
                {
                   
                    Console.WriteLine("Введите аргументы:");

                    string[] newArgs = Console.ReadLine().Split(' ');

                    ParseCommands(newArgs, out logFilePath, out outputFilePath, out addressStart, out addressMask, out timeStart, out timeEnd);
                }
            }

            if (logFilePath == null || outputFilePath == null)
            {
                Console.WriteLine("Не указан путь к файлам журналов или выходному файлу.");
                return;
            }

            IServiceCollection services = new ServiceCollection();

            services.InitializeServices();

            var serviceProvider = services.BuildServiceProvider();

            ILogAnalyzer logAnalyzer = serviceProvider.GetRequiredService<ILogAnalyzer>();

            logAnalyzer.Analyze(logFilePath, outputFilePath, addressStart, addressMask, timeStart, timeEnd);
        }

        private static IPAddress ConvertDecimalToIP(int decimalMask)
        {
            if (decimalMask < 0 || decimalMask > 32)
            {
                throw new ArgumentException("Некорректное значение маски подсети.");
            }

            string cidrMask = $"{new string('1', decimalMask)}{new string('0', 32 - decimalMask)}";

            int[] octets = {
                Convert.ToInt32(cidrMask.Substring(0, 8), 2),
                Convert.ToInt32(cidrMask.Substring(8, 8), 2),
                Convert.ToInt32(cidrMask.Substring(16, 8), 2),
                Convert.ToInt32(cidrMask.Substring(24, 8), 2)
            };

            
            IPAddress ipAddress = new IPAddress(BitConverter.GetBytes(BitConverter.ToUInt32(new byte[] {
                (byte)octets[0], (byte)octets[1], (byte)octets[2], (byte)octets[3] }, 0)));


            return ipAddress;
        }

        private static void ParseCommands(string[] args,
                                          out string logFilePath,
                                          out string outputFilePath,
                                          out IPAddress addressStart,
                                          out IPAddress addressMask,
                                          out DateTime timeStart,
                                          out DateTime timeEnd)
        {
            logFilePath = null;

            outputFilePath = null;

            addressStart = null;

            addressMask = null;

            timeStart = DateTime.MinValue;

            timeEnd = DateTime.MaxValue;

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "--file-log" when i + 1 < args.Length:
                        logFilePath = args[++i];
                        break;
                    case "--file-output" when i + 1 < args.Length:
                        outputFilePath = args[++i];
                        break;
                    case "--address-start" when i + 1 < args.Length:
                        IPAddress.TryParse(args[++i], out addressStart);
                        break;
                    case "--address-mask" when i + 1 < args.Length:
                        int.TryParse(args[++i], out int mask);
                        addressMask = IPAddress.Parse(new string('1', mask) + new string('0', 32 - mask));
                        break;
                    case "--time-start" when i + 1 < args.Length:
                        DateTime.TryParseExact(args[++i], "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out timeStart);
                        break;
                    case "--time-end" when i + 1 < args.Length:
                        DateTime.TryParseExact(args[++i], "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out timeEnd);
                        break;
                    default:
                        Console.WriteLine($"Неизвестная команда: {args[i]}");
                        return;
                     
                }
            }
        }
    }
}
