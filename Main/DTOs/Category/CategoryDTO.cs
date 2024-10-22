namespace Main.DTOs.Category;

public class CategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> Subcategories { get; set; }
    public virtual string CreatedBy { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual string? LastModifiedBy { get; set; }
    public virtual DateTime? LastModified { get; set; }
}
