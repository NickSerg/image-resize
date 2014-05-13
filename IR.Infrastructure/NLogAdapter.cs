using Microsoft.Practices.Prism.Logging;
using NLog;

namespace IR.Infrastructure
{
    public class NLogAdapter : ILoggerFacade
    {
        public void Log(string message, Category category, Priority priority)
        {
            var logger = LogManager.GetCurrentClassLogger();
            switch (category)
            {
                case Category.Debug:
                    logger.Debug(message);
                    break;
                case Category.Warn:
                    logger.Warn(message);
                    break;
                case Category.Exception:
                    logger.Error(message);
                    break;
                case Category.Info:
                    logger.Info(message);
                    break;
                default:
                    logger.Debug(message);
                    break;
            }
        }
    }
}
