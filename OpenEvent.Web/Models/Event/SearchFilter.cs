namespace OpenEvent.Web.Models.Event
{
    public class SearchFilter
    {
        public SearchParam Key { get; set; }
        public string Value { get; set; }
    }

    public enum SearchParam
    {
        Category,
        Location,
        IsOnline,
        Date
    }
}