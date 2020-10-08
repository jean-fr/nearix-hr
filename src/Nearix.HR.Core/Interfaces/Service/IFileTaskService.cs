using Nearix.HR.Core.Model;
using System.IO;
using System.Threading.Tasks;

namespace Nearix.HR.Core.Interfaces
{
    public interface IFileTaskService
    {
        Task<FileTaskResult> SaveFile(Stream inputStream, string fileName);
        Task<FileTaskResult> Import(string fileName);
        Task<FileTaskResult> Export(EmployeeSearch search);
        Task<FileTaskResult> GetCsvFileSample();
        string FileDirectory { get; }
    }
}
