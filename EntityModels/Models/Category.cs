using EntityModels.Models.Base;
#nullable disable

namespace EntityModels.Models;

public class Category : AuditableBaseEntity
{
    public string Name { get; set; }

    public IEnumerable<Subcategory> Subcategory { get; set; }
}
