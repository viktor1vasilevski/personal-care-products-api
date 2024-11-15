namespace Main.Requests;

public class SubcategoryRequest
{
    public string? Name { get; set; }
    public string? Category { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Sort { get; set; }
}
