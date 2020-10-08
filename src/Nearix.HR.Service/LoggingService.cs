using Nearix.HR.Core.Interfaces;
using NLog;
using System;

namespace Nearix.HR.Service
{
    public class LoggingService : ILoggingService
    {
        /*Keep this service simple for the sake f this test*/
        private readonly Logger _logger;
        private static bool nlogConfigSet;
        public LoggingService()
        {           
            this._logger = LogManager.GetLogger("NearixHr");
        }

        public void Error(string message)
        {
            this._logger.Error(message);
        }
        public void Warn(string message)
        {
            this._logger.Warn(message);
        }
        public void Error(Exception ex)
        {
            string msg = this.BuildExceptionMessage(ex);
            if (ex.InnerException != null)
            {
                msg += "\n" + this.BuildExceptionMessage(ex.InnerException);
            }
            this._logger.Error(ex, msg);
        }

        public void Fatal(string message)
        {
            this._logger.Fatal(message);
        }

        private string BuildExceptionMessage(Exception x)
        {
            Exception logException = x;

            const string HtmlBrakeLine = "<br/>";
            // Get the error message
            string strErrorMsg = "Message :" + logException.Message;

            // Source of the message
            strErrorMsg += HtmlBrakeLine + "Source :" + logException.Source;

            // Stack Trace of the error
            strErrorMsg += HtmlBrakeLine + "Stack Trace :" + logException.StackTrace;

            strErrorMsg += HtmlBrakeLine + "TargetSite :" + logException.TargetSite;
            return strErrorMsg;
        }

        public void Info(string message)
        {
            this._logger.Info(message);
        }
    }
}
