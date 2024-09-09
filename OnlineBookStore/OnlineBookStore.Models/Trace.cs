namespace OnlineBookStore.Models
{
    public class Trace
    {
        public int Id { get; set; }
        public string TraceId { get; set; }
        public string SpanId { get; set; }
        public string ParentSpanId { get; set; }
        public string OperationName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }
}
