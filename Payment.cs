namespace react_example
{
    public class Payment
    {
        public int Id { get; set; }
        public int OriginAccount { get; set; }
        public int DestinationAccount { get; set; }
        public string Amount { get; set; }
        public string PaymentStatus { get; set; }
    }
}