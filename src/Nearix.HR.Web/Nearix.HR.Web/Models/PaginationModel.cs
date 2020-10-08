using System;

namespace Nearix.HR.Web.Models
{
    public class PaginationModel
    {
        public PaginationModel(int pageSize)
        {
            this.DefaultPageSize = pageSize;
        }
        public PaginationModel() { }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageNumber { get; set; }
        public bool IsNeeded => this.TotalCount > this.DefaultPageSize;
        public int TotalPages => (int)Math.Ceiling((double)this.TotalCount / this.DefaultPageSize);
        public int DefaultPageSize { get; }
    }
}
