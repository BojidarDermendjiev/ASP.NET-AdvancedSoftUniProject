namespace ServerAspNetCoreAPIMakePC.Tests.ModelBinders
{
    using System;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
    using NUnit.Framework;

    using API.ModelBinders;

    [TestFixture]
    public class DecimalModelBinderProviderTests
    {
        [Test]
        public void GetBinder_ForDecimalType_ReturnsDecimalModelBinder()
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            var context = new TestModelBinderProviderContext(metadataProvider, metadataProvider.GetMetadataForType(typeof(decimal)));

            var provider = new DecimalModelBinderProvider();
            var binder = provider.GetBinder(context);

            Assert.IsNotNull(binder);
            Assert.IsInstanceOf<DecimalModelBinder>(binder);
        }

        [Test]
        public void GetBinder_ForNonDecimalType_ReturnsNull()
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            var context = new TestModelBinderProviderContext(metadataProvider, metadataProvider.GetMetadataForType(typeof(int)));

            var provider = new DecimalModelBinderProvider();
            var binder = provider.GetBinder(context);

            Assert.IsNull(binder);
        }

        class TestModelBinderProviderContext : ModelBinderProviderContext
        {
            public override BindingInfo BindingInfo => new BindingInfo();
            public override ModelMetadata Metadata { get; }
            public override IModelMetadataProvider MetadataProvider { get; }

            public TestModelBinderProviderContext(IModelMetadataProvider metadataProvider, ModelMetadata metadata)
            {
                MetadataProvider = metadataProvider;
                Metadata = metadata;
            }

            public override IModelBinder CreateBinder(ModelMetadata metadata) => null!;
        }
    }
}