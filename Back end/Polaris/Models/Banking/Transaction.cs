namespace Models.Banking
{
    public class Transaction : AmountedEntity<string, string>
    {
        public override string Id { get; set; }
        public override string Source { get; set; }
        public override string Target { get; set; }
        public override long Amount { get; set; }
        public long Date { get; set; }
        public int Time { get; set; }
        public int TrackingId { get; set; }
        public string Type { get; set; }
    }
}