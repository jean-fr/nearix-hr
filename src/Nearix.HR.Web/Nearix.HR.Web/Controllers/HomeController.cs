using System;
using System.Collections.Generic;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nearix.HR.Core.Interfaces;
using Nearix.HR.Core.Model;
using Nearix.HR.Web.Models;

namespace Nearix.HR.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggingService _loggingService;
        private readonly IMapper _mapper;
        private readonly IEmployeeDao _employeeDao;

        public HomeController(ILoggingService loggingService, IMapper mapper, IEmployeeDao employeeDao)
        {
            this._loggingService = loggingService ;
            this._employeeDao = employeeDao;
            this._mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("employee/search")]
        public IActionResult Find(EmployeeSearchModel search)
        {
            if (search == null) return new ObjectResult(new SearchResult { Success = false });

            try
            {
                SearchResult result = new SearchResult();
                var totalCount = 0;
                if (search.GetCount)
                {
                    totalCount = this._employeeDao.FindCount(this._mapper.Map<EmployeeSearch>(search));
                }
                int pn = search.Skip > 0 ? (search.Skip / search.Take) + 1 : 1;
                result.Pagination = new PaginationModel(search.Take) { TotalCount = totalCount, PageNumber = pn };
               
                List<Employee> employees =  this._employeeDao.Find(this._mapper.Map<EmployeeSearch>(search));
                result.Employees.AddRange(this._mapper.Map<List<NhEmployee>>(employees));
                result.Success = true;
                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                this._loggingService.Error(ex);
                return new ObjectResult(new SearchResult { Success = false });
            }
        }

 

        [HttpGet("import")]
        public IActionResult Import()
        {
            return View();
        }       
    }
}
