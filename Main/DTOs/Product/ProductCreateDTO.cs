using System.ComponentModel.DataAnnotations;

namespace Main.DTOs.Product;

public class ProductCreateDTO
{
    //[Required(ErrorMessage = "Product name is required.")]
    public string Name { get; set; }

    //[Required(ErrorMessage = "Brand is required.")]
    public string Brand { get; set; }

    //[Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; }

    //[Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than zero.")]
    public decimal UnitPrice { get; set; }

    //[Range(1, int.MaxValue, ErrorMessage = "Unit quantity must be greater than zero.")]
    public int UnitQuantity { get; set; }

    //[Required(ErrorMessage = "Volume is required.")]
    public int? Volume { get; set; }

    public string? Scent { get; set; }

    public string? Edition { get; set; }

    //[Required(ErrorMessage = "Image is required.")]
    public byte[]? Image { get; set; }

    public Guid SubcategoryId { get; set; }
}
