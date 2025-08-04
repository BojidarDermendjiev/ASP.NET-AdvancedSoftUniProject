namespace ServerAspNetCoreAPIMakePC.API.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    using static Domain.ErrorMessages.ErrorMessages;
    public class GuidModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
           
            if (Guid.TryParse(value, out var guid))
            {
                bindingContext.Result = ModelBindingResult.Success(guid);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, string.Format(InvalidGuidFormat));
            }

            return Task.CompletedTask;

        }
    }
}
