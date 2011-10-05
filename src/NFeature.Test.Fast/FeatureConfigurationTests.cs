// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using NFeature;
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class FeatureConfigurationTests
    {
        [Test]
        public void IsEnabledInFeatureManifest_ReturnsTrue_WhenFeatureIsAvailable()
        {
            var m = new FeatureManifest<TestFeatureList> { { TestFeatureList.TestFeature1, new FeatureDescriptor<TestFeatureList>(TestFeatureList.TestFeature1) { IsAvailable = true, } } };

            Assert.That(TestFeatureList.TestFeature1.IsAvailable(m));
        }
    }
}
// ReSharper restore InconsistentNaming