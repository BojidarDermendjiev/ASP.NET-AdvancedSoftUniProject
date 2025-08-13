namespace ServerAspNetCoreAPIMakePC.Tests.ModelBinders
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
    using NUnit.Framework;

    using API.ModelBinders;

    [TestFixture]
    public class GuidModelBinderTests
    {
        [Test]
        public async Task BindModelAsync_ValidGuid_ReturnsGuid()
        {
            var binder = new GuidModelBinder();
            var guid = Guid.NewGuid();

            var query = new QueryCollection(new System.Collections.Generic.Dictionary<string, StringValues>
            {
                { "model", guid.ToString() }
            });
            var valueProvider = new QueryStringValueProvider(
                BindingSource.Query,
                query,
                System.Globalization.CultureInfo.InvariantCulture
            );

            var modelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(Guid));
            var context = new DefaultModelBindingContext
            {
                ModelName = "model",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata,
                ModelState = new ModelStateDictionary()
            };

            await binder.BindModelAsync(context);

            Assert.IsTrue(context.Result.IsModelSet);
            Assert.AreEqual(guid, context.Result.Model);
        }

        [Test]
        public async Task BindModelAsync_InvalidGuid_AddsModelError()
        {
            var binder = new GuidModelBinder();

            var query = new QueryCollection(new System.Collections.Generic.Dictionary<string, StringValues>
            {
                { "model", "invalid-guid" }
            });
            var valueProvider = new QueryStringValueProvider(
                BindingSource.Query,
                query,
                System.Globalization.CultureInfo.InvariantCulture
            );

            var modelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(Guid));
            var context = new DefaultModelBindingContext
            {
                ModelName = "model",
                ValueProvider = valueProvider,
                ModelMetadata = modelMetadata,
                ModelState = new ModelStateDictionary()
            };

            await binder.BindModelAsync(context);

            Assert.IsFalse(context.Result.IsModelSet);
            Assert.IsTrue(context.ModelState.ContainsKey("model"));
        }
    }
}