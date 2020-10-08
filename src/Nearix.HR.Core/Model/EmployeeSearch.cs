using System.Collections.Generic;

namespace Nearix.HR.Core.Model
{
    public class EmployeeSearch
    {
        public string Query { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public SortOrder SortOrder { get; set; }
        public string SortBy { get; set; }
        public List<SearchFilter> Filters { get; set; } = new List<SearchFilter>();
    }
}