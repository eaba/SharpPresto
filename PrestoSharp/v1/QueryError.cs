namespace PrestoSharp.v1
{
    public class QueryError
    {
        public string Message { get; set; }
        public string SqlState { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorName { get; set; }
        public string ErrorType { get; set; }
        public ErrorLocation ErrorLocation { get; set; }
        public FailureInfo FailureInfo { get; set; }
    }
}
