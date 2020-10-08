namespace Nearix.HR.Core.Model
{
    public class SearchFilter
    {
        public string FieldName { get; set; }
        public FilterOperator Operator { get; set; } = FilterOperator.Equals;
        public string Value { get; set; }
    }
}