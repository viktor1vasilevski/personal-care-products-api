namespace Main.Constants;

public static class ProductConstants
{
    //ENTITY_STATUS_ACTION
    public const string PRODUCT_SUCCESSFULLY_CREATED = "Product was successfully created.";
    public const string PRODUCT_ERROR_CREATING = "An error occurred while creating the product.";

    public const string ERROR_RETRIEVING_PRODUCTS = "An error occurred while retrieving the products.";

    public const string PRODUCT_SUCCESSFULLY_RETRIVED = "Product successfully retrived.";

    public const string PRODUCT_GET_BY_ID_INFO = "Product doesn't exist.";
    public const string PRODUCT_GET_BY_ID_ERROR = "An error occurred while getting product.";

    public const string PRODUCT_SUCCESSFULLY_DELETED = "Product successfully deleted.";
    public const string PRODUCT_DELETE_ERROR = "An error occurred while deleting product.";

    public const string PRODUCT_DOESNT_EXIST = "Product doesn't exist.";
    public const string PRODUCT_CANT_BE_DELETED = "Product can't be deleted. Only products with \"UNCATEGORIZED\" subcategory can be deleted.";
}