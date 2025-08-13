namespace ServerAspNetCoreAPIMakePC.Tests.ModelBinders
{
    using System.Globalization;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using Microsoft.Extensions.Primitives;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
    using NUnit.Framework;

    using API.ModelBinders;

    [TestFixture]
    public class DecimalModelBinderTests
    {
        [TestCase("123.45", 123.45)]
        [TestCase("123,45", 123.45)]
        public async Task BindModelAsync_ValidDecimal_ReturnsDecimal(string input, decimal expected)
        {
            var metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(decimal));
            var binder = new DecimalModelBinder(metadata);

            var query = new QueryCollection(new Dictionary<string, StringValues> { { "model", input } });
            var valueProvider = new QueryStringValueProvider(
                BindingSource.Query,
                query,
                CultureInfo.InvariantCulture
            );
            var context = new DefaultModelBindingContext
            {
                ModelName = "model",
                ValueProvider = valueProvider,
                ModelMetadata = metadata,
                ModelState = new ModelStateDictionary()
            };

            var originalCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
                await binder.BindModelAsync(context);

                Assert.IsTrue(context.Result.IsModelSet);
                Assert.AreEqual(expected, context.Result.Model);
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
            }
        }

        [Test]
        public async Task BindModelAsync_EmptyValue_RequiredDecimal_AddsModelError()
        {
            var metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(decimal));
            var binder = new DecimalModelBinder(metadata);

            var query = new QueryCollection(new Dictionary<string, StringValues> { { "model", "" } });
            var valueProvider = new QueryStringValueProvider(
                BindingSource.Query,
                query,
                CultureInfo.InvariantCulture
            );
            var context = new DefaultModelBindingContext
            {
                ModelName = "model",
                ValueProvider = valueProvider,
                ModelMetadata = metadata,
                ModelState = new ModelStateDictionary()
            };

            await binder.BindModelAsync(context);

            Assert.IsTrue(context.Result.IsModelSet);
            Assert.IsNull(context.Result.Model);
            Assert.IsTrue(context.ModelState.ContainsKey("model"));
        }

        [Test]
        public async Task BindModelAsync_InvalidValue_AddsModelError()
        {
            var metadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(decimal));
            var binder = new DecimalModelBinder(metadata);

            var query = new QueryCollection(new Dictionary<string, StringValues> { { "model", "not-a-decimal" } });
            var valueProvider = new QueryStringValueProvider(
                BindingSource.Query,
                query,
                CultureInfo.InvariantCulture
            );
            var context = new DefaultModelBindingContext
            {
                ModelName = "model",
                ValueProvider = valueProvider,
                ModelMetadata = metadata,
                ModelState = new ModelStateDictionary()
            };

            await binder.BindModelAsync(context);

            Assert.IsFalse(context.Result.IsModelSet);
            Assert.IsTrue(context.ModelState.ContainsKey("model"));
        }
    }
}