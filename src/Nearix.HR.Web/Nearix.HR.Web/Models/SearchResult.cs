using System.Collections.Generic;

namespace Nearix.HR.Web.Models
{
    public class SearchResult
    {
        public List<NhEmployee> Employees { get; set; } = new List<NhEmployee>();         
        public bool Success { get; set; }
        public PaginationModel Pagination { get; set; }
    }
}
