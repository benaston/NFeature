// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class FeatureConfigurationTests
    {
        [Test]
        public void IsEnabledInFeatureManifest_ReturnsTrue_WhenFeatureIsAvailable()
        {
            var m = new FeatureManifest<Feature>
                        {
                            {
                                Feature.TestFeatureA,
                                new FeatureDescriptor<Feature>(Feature.TestFeatureA)
                                    {IsAvailable = true,}
                                }
                        };

            Assert.That(Feature.TestFeatureA.IsAvailable(m));
        }
    }
}

// ReSharper restore InconsistentNaming