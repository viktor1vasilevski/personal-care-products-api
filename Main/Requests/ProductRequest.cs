namespace Main.Requests;

public class ProductRequest
{
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Edition { get; set; }
    public string? Scent { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? SubcategoryId { get; set; }
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public string? Sort { get; set; }
}
