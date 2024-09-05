using EntityModels.Models.Base;

namespace EntityModels.Models;

public class Subcategory : AuditableBaseEntity
{
    public string Name { get; set; }


    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Product> Products { get; set; }
}
