using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nearix.HR.Core.Interfaces;
using Nearix.HR.Core.Model;
using Nearix.HR.Web.Models;

namespace Nearix.HR.Web.Controllers
{
    public class FileTaskController : Controller
    {
        private readonly ILoggingService _loggingService;
        private readonly IFileTaskService _fileTaskService;
        private readonly IMapper _mapper;
        public FileTaskController(ILoggingService loggingService, IFileTaskService fileTaskService, IMapper mapper)
        {
            this._loggingService = loggingService;
            this._fileTaskService = fileTaskService;
            this._mapper = mapper;
        }

        [HttpPost("file/import")]
        public async Task<IActionResult> ImportAsync(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return new ObjectResult(new NrFileResult { Success = false, Message = "File name not provided" });

            try
            {
                var result = await this._fileTaskService.Import(fileName);
                return new ObjectResult(new NrFileResult { Success = result.Success, Message = result.Message });
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, please check the log for more details" });
            }
        }

        [HttpPost("file/export")]
        public async Task<IActionResult> ExportAsync(EmployeeSearchModel search)
        {
            if (search == null) return new ObjectResult(new NrFileResult { Success = false, Message = "search object not provided" });

            try
            {
                var result = await this._fileTaskService.Export(this._mapper.Map<EmployeeSearch>(search));
                return new ObjectResult(new NrFileResult { Success = result.Success, Message = result.Success ? result.FileName : result.Message });
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, please check the log for more details" });
            }
        }

        [HttpPost("file/upload")]
        public async Task<IActionResult> UploadAsync()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null)
            {
                return new ObjectResult(new NrFileResult { Success = false });
            }

            if (Path.GetExtension(file.FileName) != ".csv")
            {
                return new ObjectResult(new NrFileResult { Success = false, Message = "Only CSV files are accepted" });
            }

            try
            {
                string fileName = file.FileName;
                Stream fileStream = file.OpenReadStream();
                var result = await this._fileTaskService.SaveFile(fileStream, fileName);

                if (!result.Success) return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, try again" });

                return new ObjectResult(new NrFileResult { Success = true, Message = result.FileName });
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, try again" });
            }
        }

        [HttpGet("download/filesample")]
        public async Task<IActionResult> FileSampleAsync()
        {
            try
            {
                var result = await this._fileTaskService.GetCsvFileSample();
                if (!result.Success) return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, try again" });

                return this.DownloadFile(result.FileName).Result;
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, try again" });
            }
        }

        [HttpGet("download/exported/{fileName}")]
        public IActionResult ExportFile(string fileName)
        {
            try
            {
                return this.DownloadFile(fileName).Result;
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, try again" });
            }
        }

        private async Task<IActionResult> DownloadFile(string fileName)
        {
            try
            {         
                if (string.IsNullOrWhiteSpace(fileName)) return new ObjectResult(new NrFileResult { Success = false, Message = "File path not provided" });
                var filePath = Path.Combine(this._fileTaskService.FileDirectory, fileName);
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, "text/csv", Path.GetFileName(filePath));
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return new ObjectResult(new NrFileResult { Success = false, Message = "Something went wrong, try again" });
            }
        }
    }
}
