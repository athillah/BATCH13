namespace demoStructuredLogging.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public string? CorrelationId { get; set; }
    }

    public class PaginatedRequest
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
        public string Filter { get; set; } = string.Empty;
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}