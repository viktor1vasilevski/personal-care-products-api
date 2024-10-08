namespace Main.DTOs.Responses;

public class BaseResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; } = string.Empty;
    public string? ExceptionMessage { get; set; } = string.Empty;
}
