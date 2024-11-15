namespace Main.Requests;

public class SubcategoryRequest
{
    public string? Name { get; set; }
    public Guid? CategoryId { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Sort { get; set; }
}
