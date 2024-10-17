﻿namespace Main.Responses;

public class SingleResponse<T> where T : class
{
    public T Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; } = string.Empty;
    public string? ExceptionMessage { get; set; } = string.Empty;
}
