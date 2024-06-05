using EntityModels.Models.Base;
#nullable disable

namespace EntityModels.Models;

public class Subcategory : AuditableBaseEntity
{
    public string Name { get; set; }


    public Guid CategoryId { get; set; }
    public Category Category { get; set; }

    public IEnumerable<Product> Products { get; set; }
}
