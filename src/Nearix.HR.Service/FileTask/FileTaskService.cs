using Nearix.HR.Core.Interfaces;
using Nearix.HR.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Nearix.HR.Service
{
    public class FileTaskService : IFileTaskService
    {
        private readonly string _uploadDirectory;
        private readonly ILoggingService _loggingService;
        private readonly IEmployeeDao _employeeDao;
        private readonly string[] _headerKeys;
        private readonly string _headersStr;

        public string FileDirectory => this._uploadDirectory;

        public FileTaskService(string uploadDirectory, IEmployeeDao employeeDao, ILoggingService loggingService)
        {
            if (string.IsNullOrWhiteSpace(uploadDirectory))
            {
                throw new ArgumentException($"{uploadDirectory} argument must be provided");
            }
            this._uploadDirectory = uploadDirectory;
            this._employeeDao = employeeDao;
            this._loggingService = loggingService;
            this._headerKeys = EmployeeMapping.Map.Select(x => x.Key).ToArray();
            this._headersStr = this.BuilHeaders();
        }

        public Task<FileTaskResult> Export(EmployeeSearch search)
        {
            if (search == null)
            {
                this._loggingService.Error(new ArgumentNullException("Search object is required"));
                return Task.FromResult(new FileTaskResult { Success = false, Message = "No search passed to export" });
            }

            try
            {
                var fileName = $"employees-export-{DateTime.UtcNow:dd-MM-yyyy-HHmmss}.csv";
                var outputFile = Path.Combine(this._uploadDirectory, fileName);

                using (StreamWriter strWriter = new StreamWriter(outputFile, false, Encoding.Default))
                {
                    //headers first
                    strWriter.WriteLine(this._headersStr);
                    search.Skip = 0;
                    search.Take = 50;
                    var employees = this._employeeDao.Find(search);
                    var totalCount = this._employeeDao.FindCount(search);

                    while (employees.Count > 0)
                    {
                        foreach (var employee in employees)
                        {
                            this.ProcessFile(strWriter, employee);
                        }
                        search.Skip += employees.Count;
                        if (search.Skip < totalCount)
                        {
                            employees = this._employeeDao.Find(search);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                return Task.FromResult(new FileTaskResult { Success = true, FileName = fileName });
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return Task.FromResult(new FileTaskResult { Success = false, Message = "An error has occured while exporting file. check the log for more details" });
            }
        }

        private string BuilHeaders()
        {
            var sb = new StringBuilder();
            foreach (var header in EmployeeMapping.Map.Select(x => x.Value))
            {
                sb.AppendFormat(sb.Length == 0 ? "\"{0}\"" : ",\"{0}\"", header);
            }
            return sb.ToString();
        }

        private void ProcessFile(StreamWriter sw, Employee employee)
        {
            if (sw == null || employee == null) return;

            var sb = new StringBuilder();
            for (var i = 0; i < this._headerKeys.Length; i++)
            {
                string cellValue;
                switch (this._headerKeys[i])
                {
                    case "employeeid":
                        cellValue = employee.EmployeeId.ToString();
                        break;
                    case "firstname":
                        cellValue = employee.FirstName;
                        break;
                    case "lastname":
                        cellValue = employee.LastName;
                        break;
                    case "username":
                        cellValue = employee.UserName;
                        break;
                    case "password":
                        cellValue = employee.Password;
                        break;
                    case "nameprefix":
                        cellValue = employee.NamePrefix;
                        break;
                    case "middleinitial":
                        cellValue = employee.MiddleInitial;
                        break;
                    case "gender":
                        cellValue = employee.Gender;
                        break;
                    case "email":
                        cellValue = employee.Email;
                        break;
                    case "fathername":
                        cellValue = employee.FatherName;
                        break;
                    case "mothermaidenname":
                        cellValue = employee.MotherMaidenName;
                        break;
                    case "mothername":
                        cellValue = employee.MotherName;
                        break;
                    case "dateofbirth":
                        cellValue = employee.DateOfBirth?.ToString("mm/dd/yy");
                        break;
                    case "timeofbirth":
                        cellValue = employee.TimeOfBirth;
                        break;
                    case "ageinyears":
                        cellValue = employee.AgeInYears.ToString();
                        break;
                    case "weightinkgs":
                        cellValue = employee.WeightInKgs.ToString();
                        break;
                    case "dateofjoining":
                        cellValue = employee.DateOfJoining?.ToString("mm/dd/yy");
                        break;
                    case "quarterofjoining":
                        cellValue = employee.QuarterOfJoining;
                        break;
                    case "halfofjoining":
                        cellValue = employee.HalfOfJoining;
                        break;
                    case "yearofjoining":
                        cellValue = employee.YearOfJoining.ToString();
                        break;
                    case "monthofjoining":
                        cellValue = employee.MonthOfJoining.ToString();
                        break;
                    case "monthnameofjoining":
                        cellValue = employee.MonthNameOfJoining;
                        break;
                    case "shortmonth":
                        cellValue = employee.ShortMonth;
                        break;
                    case "dayofjoining":
                        cellValue = employee.DayOfJoining.ToString();
                        break;
                    case "dowofjoining":
                        cellValue = employee.DowOfJoining;
                        break;
                    case "shortdow":
                        cellValue = employee.ShortDow;
                        break;
                    case "ageincompanyinyears":
                        cellValue = employee.AgeInCompanyInYears.ToString();
                        break;
                    case "salary":
                        cellValue = employee.Salary.ToString();
                        break;
                    case "ssn":
                        cellValue = employee.Ssn;
                        break;
                    case "lasthike":
                        cellValue = $"{employee.LastHike.ToString()}%";
                        break;
                    case "phonenumber":
                        cellValue = employee.PhoneNumber;
                        break;
                    case "placename":
                        cellValue = employee.PlaceName;
                        break;
                    case "county":
                        cellValue = employee.County;
                        break;
                    case "city":
                        cellValue = employee.City;
                        break;
                    case "state":
                        cellValue = employee.State;
                        break;
                    case "zip":
                        cellValue = employee.Zip;
                        break;
                    case "region":
                        cellValue = employee.Region;
                        break;
                    default:
                        cellValue = string.Empty;
                        break;
                }

                sb.AppendFormat(sb.Length == 0 ? "\"{0}\"" : ",\"{0}\"", cellValue);
            }

            sw.WriteLine(sb.ToString());
        }

        public Task<FileTaskResult> GetCsvFileSample()
        {
            //check sample, if not exists create
            try
            {
                var sampleName = "employees_sample.csv";
                var file = Path.Combine(this._uploadDirectory, sampleName);
                if (File.Exists(file)) return Task.FromResult(new FileTaskResult { Success = true, FileName = file });

                //generate sample headers only and save
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLineAsync(this._headersStr);
                }
                return Task.FromResult(new FileTaskResult { Success = true, FileName = file });
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return Task.FromResult(new FileTaskResult { Success = false, Message = "An error has occured while importing file. check the log for more details" });
            }
        }

        public Task<FileTaskResult> Import(string fileName)
        {
            try
            {
                var file = Path.Combine(this._uploadDirectory, fileName);
                if (!File.Exists(file))
                {
                    this._loggingService.Error($"File {fileName} not found");
                    return Task.FromResult(new FileTaskResult { Success = false, Message = "File Not Found" });
                }

                if (Path.GetExtension(file) != (".csv"))
                {
                    this._loggingService.Error("Wrong file type");
                    File.Delete(file);
                    return Task.FromResult(new FileTaskResult { Success = false, Message = "Wrong file type. Must be CSV file only" });
                }
                int proceeded = 0;
                int total = 0;

                var records = File.ReadAllLinesAsync(file).Result;
                if (!records.Any())
                {
                    this._loggingService.Error("The provided file is empty");
                    File.Delete(file);
                    return Task.FromResult(new FileTaskResult { Success = false, Message = "The provided file is empty" });
                }

                string[] headers = records[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                if (!HeadersAreValid(headers))
                {
                    this._loggingService.Error("Wrong file headers");
                    File.Delete(file);
                    return Task.FromResult(new FileTaskResult { Success = false, Message = "Wrong file header. Make sure to use the file sample" });
                }

                var employees = this.GetEmployees(records);
                total = employees.Count();

                Parallel.ForEach(employees, e =>
                {
                    if (this._employeeDao.Exists(e.EmployeeId))
                    {
                        this._loggingService.Warn($"Employee whith Id: <{e.EmployeeId} | {e.Email}> Already Exsts into the DB");
                    }
                    else
                    {
                        if (this._employeeDao.Save(e))
                        {
                            proceeded++;
                        }
                        else
                        {
                            this._loggingService.Error($"Employee whith Id: <{e.EmployeeId} | {e.Email}> was not saved into the DB");
                        }
                    }
                });

                var message = $"Import completed [ Total: {total} | Proceeded : {proceeded}]";
                this._loggingService.Info(message);
                File.Delete(file);
                return Task.FromResult(new FileTaskResult { Success = true, Message = message });
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return Task.FromResult(new FileTaskResult { Success = false, Message = "An error has occured while importing file. Make sure using the provided sample. Check the log for more details" });
            }
        }

        private List<Employee> GetEmployees(string[] records)
        {
            string[] headers = records[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            var h = headers.Select(x => GetHeaderKey(x)).ToArray();
            var employees = new List<Employee>();
            Parallel.For(1, records.Length, i =>
             {
                 var line = records[i];
                 var data = line.Split(new string[] { "," }, StringSplitOptions.None);

                 Employee employee = new Employee();

                 employee.FirstName = data[Array.IndexOf(h, nameof(employee.FirstName).ToLower())].Trim();
                 employee.LastName = data[Array.IndexOf(h, nameof(employee.LastName).ToLower())].Trim();
                 employee.UserName = data[Array.IndexOf(h, nameof(employee.UserName).ToLower())].Trim();
                 employee.Password = data[Array.IndexOf(h, nameof(employee.Password).ToLower())].Trim();
                 employee.NamePrefix = data[Array.IndexOf(h, nameof(employee.NamePrefix).ToLower())].Trim();
                 employee.MiddleInitial = data[Array.IndexOf(h, nameof(employee.MiddleInitial).ToLower())].Trim();
                 employee.Gender = data[Array.IndexOf(h, nameof(employee.Gender).ToLower())].Trim();
                 employee.Email = data[Array.IndexOf(h, nameof(employee.Email).ToLower())].Trim();
                 employee.FatherName = data[Array.IndexOf(h, nameof(employee.FatherName).ToLower())].Trim();
                 employee.MotherName = data[Array.IndexOf(h, nameof(employee.MotherName).ToLower())].Trim();
                 employee.MotherMaidenName = data[Array.IndexOf(h, nameof(employee.MotherMaidenName).ToLower())].Trim();
                 employee.Ssn = data[Array.IndexOf(h, nameof(employee.Ssn).ToLower())].Trim();
                 employee.PhoneNumber = data[Array.IndexOf(h, nameof(employee.PhoneNumber).ToLower())].Trim();
                 employee.PlaceName = data[Array.IndexOf(h, nameof(employee.PlaceName).ToLower())].Trim();
                 employee.County = data[Array.IndexOf(h, nameof(employee.County).ToLower())].Trim();
                 employee.City = data[Array.IndexOf(h, nameof(employee.City).ToLower())].Trim();
                 employee.State = data[Array.IndexOf(h, nameof(employee.State).ToLower())].Trim();
                 employee.Zip = data[Array.IndexOf(h, nameof(employee.Zip).ToLower())].Trim();
                 employee.Region = data[Array.IndexOf(h, nameof(employee.Region).ToLower())].Trim();
                 employee.QuarterOfJoining = data[Array.IndexOf(h, nameof(employee.QuarterOfJoining).ToLower())].Trim();
                 employee.HalfOfJoining = data[Array.IndexOf(h, nameof(employee.HalfOfJoining).ToLower())].Trim();
                 employee.ShortMonth = data[Array.IndexOf(h, nameof(employee.ShortMonth).ToLower())].Trim();
                 employee.ShortDow = data[Array.IndexOf(h, nameof(employee.ShortDow).ToLower())].Trim();
                 employee.DowOfJoining = data[Array.IndexOf(h, nameof(employee.DowOfJoining).ToLower())].Trim();
                 employee.MonthNameOfJoining = data[Array.IndexOf(h, nameof(employee.MonthNameOfJoining).ToLower())].Trim();
                 employee.TimeOfBirth = data[Array.IndexOf(h, nameof(employee.TimeOfBirth).ToLower())].Trim();

                 if (int.TryParse(data[Array.IndexOf(h, nameof(employee.EmployeeId).ToLower())].Trim(), out int id))
                 {
                     employee.EmployeeId = id;
                 }
                 if (DateTime.TryParse(data[Array.IndexOf(h, nameof(employee.DateOfBirth).ToLower())].Trim(), out DateTime d))
                 {
                     employee.DateOfBirth = d.Date;
                 }

                 if (DateTime.TryParse(data[Array.IndexOf(h, nameof(employee.DateOfJoining).ToLower())].Trim(), out DateTime dj))
                 {
                     employee.DateOfJoining = dj;
                 }
                 if (double.TryParse(data[Array.IndexOf(h, nameof(employee.AgeInYears).ToLower())].Trim(), out double ay))
                 {
                     employee.AgeInYears = ay;
                 }
                 if (double.TryParse(data[Array.IndexOf(h, nameof(employee.WeightInKgs).ToLower())].Trim().Trim(), out double w))
                 {
                     employee.WeightInKgs = w;
                 }
                 if (int.TryParse(data[Array.IndexOf(h, nameof(employee.YearOfJoining).ToLower())].Trim(), out int y))
                 {
                     employee.YearOfJoining = y;
                 }
                 if (int.TryParse(data[Array.IndexOf(h, nameof(employee.MonthOfJoining).ToLower())].Trim(), out int m))
                 {
                     employee.MonthOfJoining = m;
                 }
                 var lhk = data[Array.IndexOf(h, nameof(employee.LastHike).ToLower())].Trim();
                 lhk = lhk.Replace("%", "");

                 if (int.TryParse(lhk, out int lh))
                 {
                     employee.LastHike = lh;
                 }
                 if (int.TryParse(data[Array.IndexOf(h, nameof(employee.DayOfJoining).ToLower())].Trim(), out int doj))
                 {
                     employee.DayOfJoining = doj;
                 }
                 if (double.TryParse(data[Array.IndexOf(h, nameof(employee.AgeInCompanyInYears).ToLower())].Trim(), out double ac))
                 {
                     employee.AgeInCompanyInYears = ac;
                 }
                 if (decimal.TryParse(data[Array.IndexOf(h, nameof(employee.Salary).ToLower())].Trim(), out decimal s))
                 {
                     employee.Salary = s;
                 }
                 employees.Add(employee);
             });

            return employees;
        }

        private bool HeadersAreValid(string[] headers)
        {
            var h = headers.Select(x => GetHeaderKey(x));
            return h.All(x => EmployeeMapping.Map.ContainsKey(x));
        }

        private string GetHeaderKey(string header)
        {
            if (string.IsNullOrWhiteSpace(header)) return string.Empty;
            return Regex.Replace(header.ToLower(), @"\s", "");            
        }

        public async Task<FileTaskResult> SaveFile(Stream inputStream, string fileName)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(this._uploadDirectory);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }

                string path = Path.Combine(this._uploadDirectory, fileName);
                using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
                {
                  await  inputStream.CopyToAsync(outputFileStream);
                }

                return await Task.FromResult(new FileTaskResult { Success = true, FileName = fileName });
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return await Task.FromResult(new FileTaskResult { Success = false, Message = "An error has occured while saving file. check the log for more details" });
            }
        }


    }
}
