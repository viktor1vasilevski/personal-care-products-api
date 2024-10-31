namespace Main.DTOs.Subcategory;

public class SubcategoryDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> Products { get; set; }
    public virtual string CreatedBy { get; set; }
    public virtual DateTime Created { get; set; }
    public virtual string? LastModifiedBy { get; set; }
    public virtual DateTime? LastModified { get; set; }
}
