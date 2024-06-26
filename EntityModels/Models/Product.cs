﻿using EntityModels.Models.Base;

namespace EntityModels.Models;

public class Product : AuditableBaseEntity
{
    public string Brand { get; set; }
    public string Description { get; set; }
    public int UnitPrice { get; set; }
    public int UnitQuantity { get; set; }
    public int? Volume { get; set; }
    public string? Scent { get; set; }
    public string? Edition { get; set; }

    public Guid SubcategoryId { get; set; }
    public Subcategory Subcategory { get; set; }
}
