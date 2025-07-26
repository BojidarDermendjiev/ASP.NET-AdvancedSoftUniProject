namespace ServerAspNetCoreAPIMakePC.Domain.Constants
{
    public class ProductValidationConstants
    {
        public const int ProductNameMinLength = 2;
        public const int ProductNameMaxLength = 100;
        public const int ProductTypeMinLength = 2;
        public const int ProductTypeMaxLength = 50;
        public const int ProductDescriptionMinLength = 50;
        public const int ProductDescriptionMaxLength = 1000;
        public const int ProductSpecsMinLength = 10;
        public const int ProductSpecsMaxLength = 2000;
        public const int ProductStockMinValue = 10;
        public const int ProductStockMaxValue = 500;
        public const double ProductPriceMinValue = 0.01;
        public const double ProductPriceMaxValue = double.MaxValue;
    }
}
