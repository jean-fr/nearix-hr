using Nearix.HR.Core.Model;
using System.Collections.Generic;

namespace Nearix.HR.Web.Models
{
    public class EmployeeSearchModel
    {
        public string Query { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public Core.SortOrder SortOrder { get; set; }
        public string SortBy { get; set; }
        public bool GetCount { get; set; }
        public List<SearchFilter> Filters { get; set; }
    }
}