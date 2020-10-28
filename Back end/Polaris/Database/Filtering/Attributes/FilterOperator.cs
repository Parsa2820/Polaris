namespace Database.Filtering.Attributes
{
    public class FilterOperator : System.Attribute
    {
        public FilterOperator(string abbrv)
        {
            Abbrv = abbrv;
        }

        public string Abbrv { get; }
    }
}