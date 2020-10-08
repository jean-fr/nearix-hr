using Nearix.HR.Core;

namespace Nearix.HR.Web.Models
{
    public class NhSearchFilter
    {
        public string FieldName { get; set; }
        public FilterOperator Operator { get; set; } = FilterOperator.Equals;
        public object Value { get; set; }
    }
}