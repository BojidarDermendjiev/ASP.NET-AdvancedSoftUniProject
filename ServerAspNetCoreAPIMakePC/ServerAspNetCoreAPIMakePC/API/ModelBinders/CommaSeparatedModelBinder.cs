namespace ServerAspNetCoreAPIMakePC.API.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public class CommaSeparatedModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (string.IsNullOrEmpty(value))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }

            var elements = value.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .ToList();

            bindingContext.Result = ModelBindingResult.Success(elements);
            return Task.CompletedTask;
        }
    }
}
