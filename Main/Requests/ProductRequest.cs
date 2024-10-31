namespace Main.Requests;

public class ProductRequest
{
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Sort { get; set; }
}
