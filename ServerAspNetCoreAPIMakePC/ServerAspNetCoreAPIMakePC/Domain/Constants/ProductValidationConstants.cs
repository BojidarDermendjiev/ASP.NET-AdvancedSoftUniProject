namespace ServerAspNetCoreAPIMakePC.Domain.Constants
{
    public class ProductValidationConstants
    {
        public const int ProductNameMinLength = 2;
        public const int ProductNameMaxLength = 100;
        public const int ProductTypeMinLength = 2;
        public const int ProductTypeMaxLength = 50;
        public const int ProductBrandMinLength = 2;
        public const int ProductBrandMaxLength = 50;
        public const int ProductDescriptionMinLength = 50;
        public const int ProductDescriptionMaxLength = 1000;
        public const int ProductSpecsMinLength = 10;
        public const int ProductSpecsMaxLength = 2000;
        public const int ProductStockMinValue = 10;
        public const int ProductStockMaxValue = 500;
    }
}
