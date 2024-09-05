using EntityModels.Models.Base;

namespace EntityModels.Models;

public class Category : AuditableBaseEntity
{
    public string Name { get; set; }

    public ICollection<Subcategory> Subcategory { get; set; }
}
