using System;
namespace Nearix.HR.Core.Interfaces
{
    /*Could be extended to more interfaces, keep it simple for this test*/
    public interface ILoggingService
    {
        void Error(string message);
        void Error(Exception ex);
        void Fatal(string message);
        void Warn(string message);
        void Info(string message);
    }
}
