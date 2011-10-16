// ReSharper disable InconsistentNaming
namespace NFeature.Test.Slow
{
    using System;
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
                new FeatureSettingAvailabilityChecker<Feature, Tenant>(MyAvailabilityCheckFunction);
            var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature, Tenant>();
            var featureSettingService =
                new FeatureSettingService<Feature, Tenant, EmptyArgs>(availabilityChecker, featureSettingRepo);
            var manifestCreationStrategy = new MyManifestCreationStrategy(featureSettingRepo, featureSettingService);
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
            return Enum.GetName(typeof(Feature), s.Feature) == "TestFeatureE";
        }

        private class MyManifestCreationStrategy : IFeatureManifestCreationStrategy<Feature>
        {
            private readonly IFeatureSettingRepository<Feature, Tenant> _featureSettingRepository;
            private readonly IFeatureSettingService<Feature, Tenant, EmptyArgs> _featureSettingService;

            public MyManifestCreationStrategy(IFeatureSettingRepository<Feature, Tenant> featureSettingRepository,
                                                IFeatureSettingService<Feature, Tenant, EmptyArgs> featureSettingService)
            {
                _featureSettingRepository = featureSettingRepository;
                _featureSettingService = featureSettingService;
            }

            public IFeatureManifest<Feature> CreateFeatureManifest()
            {
                var featureSettings = _featureSettingRepository.GetFeatureSettings();
                var manifest = new FeatureManifest<Feature>();

                foreach (var setting in featureSettings)
                {
                    var isAvailable = _featureSettingService
                        .AllDependenciesAreSatisfiedForTheFeatureSetting(setting, new EmptyArgs());

                    manifest.Add(setting.Feature,
                                 new FeatureDescriptor<Feature>(setting.Feature)
                                 {
                                     Dependencies = setting.Dependencies,
                                     IsAvailable = isAvailable,
                                     Settings = setting.Settings,
                                 });
                }

                return manifest;
            }
        }
    }
}
// ReSharper restore InconsistentNaming