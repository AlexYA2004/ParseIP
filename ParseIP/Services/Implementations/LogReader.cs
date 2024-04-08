using ParseIP.Services.Interfaces;
namespace ParseIP.Services.Implementations
{
    public class LogReader : ILogReader
    {
        public IEnumerable<string> Read(string logFilePath)
        {
            if (!File.Exists(logFilePath))
            {
                throw new FileNotFoundException($"Файл не найден: {logFilePath}");
            }

            using (var reader = new StreamReader(logFilePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
