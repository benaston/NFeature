// ReSharper disable InconsistentNaming
namespace NFeature.Test.Slow
{
    using System;
    using DefaultImplementations;
    using NUnit.Framework;

    [TestFixture]
    [Category("Slow")]
    public class FeatureEnumExtensionsTests
    {
        IFeatureManifest<Feature> _featureManifest;

        [SetUp]
        public void Setup()
        {
            var availabilityChecker =
                new FeatureSettingAvailabilityChecker<Feature, EmptyArgs, Tenant>(MyAvailabilityCheckFunction);
            var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature, Tenant>();
            var featureSettingService =
                new FeatureSettingService<Feature, Tenant, EmptyArgs>(availabilityChecker, featureSettingRepo);
            var manifestCreationStrategy = new ManifestCreationStrategyDefault<Feature, Tenant>(featureSettingRepo, featureSettingService);
            var featureManifestService = new FeatureManifestService<Feature>(manifestCreationStrategy);
            _featureManifest = featureManifestService.GetManifest();
        }

        [Test]
        public void IsAvailable_WhenTheAvailabilityCheckingFunctionReturnsTrueAndDependenciesAreOK_ReturnsTrue()
        {
            Assert.That(Feature.TestFeatureE.IsAvailable(_featureManifest));
        }

        [Test]
        public void IsAvailable_WhenTheAvailabilityCheckingFunctionReturnsFalse_ReturnsFalse()
        {
            Assert.That(!Feature.TestFeatureA.IsAvailable(_featureManifest));
            Assert.That(!Feature.TestFeatureB.IsAvailable(_featureManifest));
            Assert.That(!Feature.TestFeatureC.IsAvailable(_featureManifest));
            Assert.That(!Feature.TestFeatureD.IsAvailable(_featureManifest));
        }

        /// <summary>
        /// A function to test the availability checking behavior.
        /// </summary>
        private static bool MyAvailabilityCheckFunction(FeatureSetting<Feature, Tenant> s, EmptyArgs args)
        {
            Func<bool> f = () => true;
            return Enum.GetName(typeof(Feature), s.Feature) == "TestFeatureE";
        }
    }
}
// ReSharper restore InconsistentNaming