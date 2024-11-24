namespace Main.Requests.Base;

public class BaseQueryRequest
{
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Sort { get; set; }
}
