namespace ServerAspNetCoreAPIMakePC.API.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System.Globalization;
    public class DecimalModelBinder : IModelBinder
    {
        private readonly ModelMetadata _metadata;

        public DecimalModelBinder(ModelMetadata modelMetadata)
        {
            this._metadata = modelMetadata ;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            if (valueResult == ValueProviderResult.None || string.IsNullOrWhiteSpace(valueResult.FirstValue))
            {
                if (this._metadata.IsRequired && this._metadata.ModelType == typeof(decimal))
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, "A value is required.");
                }
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var input = valueResult.FirstValue.Trim();

            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            
            input = separator == "." ? input.Replace(",", ".") : input.Replace(".", ",");

            if (decimal.TryParse(input, NumberStyles.Number, CultureInfo.CurrentCulture, out var parsedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(parsedValue);
            }
            else
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, $"The value '{valueResult.FirstValue}' is not a valid decimal number.");
            }

            return Task.CompletedTask;
        }
    }
}
