using Main.Enums;

namespace Main.Responses;

public class QueryResponse<T> where T : class
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; } = string.Empty;
    public string? ExceptionMessage { get; set; } = string.Empty;
    public int TotalCount { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }
    public NotificationType NotificationType { get; set; }
}
