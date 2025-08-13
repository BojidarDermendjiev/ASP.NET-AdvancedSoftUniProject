namespace ServerAspNetCoreAPIMakePC.Tests.ModelBinders
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using Microsoft.Extensions.Primitives;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
    using NUnit.Framework;

    using API.ModelBinders;

    [TestFixture]
    public class CommaSeparatedModelBinderTests
    {
        [Test]
        public async Task BindModelAsync_WithCommaSeparatedString_ReturnsList()
        {
            var binder = new CommaSeparatedModelBinder();
            var query = new QueryCollection(new Dictionary<string, StringValues>
            {
                { "model", "a, b, c" }
            });
            var valueProvider = new QueryStringValueProvider(
                BindingSource.Query,
                query,
                System.Globalization.CultureInfo.InvariantCulture
            );
            var modelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(List<string>));

            var context = new DefaultModelBindingContext
            {
                ModelName = "model",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            await binder.BindModelAsync(context);

            Assert.IsTrue(context.Result.IsModelSet);
            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, (List<string>)context.Result.Model);
        }

        [Test]
        public async Task BindModelAsync_WithEmptyString_ReturnsNull()
        {
            var binder = new CommaSeparatedModelBinder();
            var query = new QueryCollection(new Dictionary<string, StringValues>
            {
                { "model", "" }
            });
            var valueProvider = new QueryStringValueProvider(
                BindingSource.Query,
                query,
                System.Globalization.CultureInfo.InvariantCulture
            );
            var modelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(List<string>));

            var context = new DefaultModelBindingContext
            {
                ModelName = "model",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata
            };

            await binder.BindModelAsync(context);

            Assert.IsTrue(context.Result.IsModelSet);
            Assert.IsNull(context.Result.Model);
        }
    }
}