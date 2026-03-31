// Logging/ErrorLogger.cs
using System.Text;

namespace MovieAPI.Logging
{
    public static class ErrorLogger
    {
        private static readonly string _logPath = Path.Combine(Directory.GetCurrentDirectory(), "ERRORS", "reports.txt");
        private static readonly object _lock = new object();

        static ErrorLogger()
        {
            // Создаем папку Errors если её нет
            var directory = Path.GetDirectoryName(_logPath);
            if (!Directory.Exists(directory) && directory != null)
            {
                Directory.CreateDirectory(directory);
            }
        }

        public static void LogError(string message, Exception? ex = null)
        {
            lock (_lock) //Тут я ограничил доступ к файлу, чтобы избежать проблем при одновременной записи из разных потоков
            {
                try
                {
                    var logEntry = new StringBuilder();
                    logEntry.AppendLine($"=== ЗАПИСЬ ОБ ОШИБКЕ ===");
                    logEntry.AppendLine($"Время: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                    logEntry.AppendLine($"Сообщение: {message}");
                    
                    if (ex != null)
                    {
                        logEntry.AppendLine($"Exception: {ex.GetType().Name}");
                        logEntry.AppendLine($"Error: {ex.Message}");
                        logEntry.AppendLine($"Stack Trace: {ex.StackTrace}");
                        
                        if (ex.InnerException != null)
                        {
                            logEntry.AppendLine($"Inner Exception: {ex.InnerException.Message}");
                        }
                    }
                    
                    logEntry.AppendLine(new string('-', 60));
                    logEntry.AppendLine();
                    
                    File.AppendAllText(_logPath, logEntry.ToString(), Encoding.UTF8);
                }
                catch
                {
                    
                }
            }
        }
    }
}