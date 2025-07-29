namespace ServerAspNetCoreAPIMakePC.Domain.Constants
{
    public class OrderValidationConstants
    {
        public const int OrderShippingAddressMinLength = 10;
        public const int OrderShippingAddressMaxLength = 300;
        public const int OrderPaymentStatusMinLength = 10;
        public const int OrderPaymentStatusMaxLength = 50;
        public const double OrderTotalPriceMinValue = 0.01;
        public const double OrderTotalPriceMaxValue = double.MaxValue;
        public const int OrderQuantityMinValue = 1;
        public const int OrderQuantityMaxValue = int.MaxValue;
        public const double OrderUnitPriceMinValue = 0.01;
        public const double OrderUnitPriceMaxValue = double.MaxValue;
      
            
    }
}
