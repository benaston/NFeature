//uncomment to test with App.config-no-tenancy
//// ReSharper disable InconsistentNaming
//namespace NFeature.Test.Slow
//{
//    using System;
//    using Configuration;
//    using DefaultImplementations;
//    using NUnit.Framework;

//    [TestFixture]
//    [Category("Slow")]
//    public class FeatureEnumExtensionsTestsWithoutTenancy
//    {
//        IFeatureManifest<Feature> _featureManifest;

//        [SetUp]
//        public void Setup()
//        {
//            var availabilityChecker =
//                new FeatureSettingAvailabilityChecker<Feature>(MyAvailabilityCheckFunction);
//            var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature>();
//            var featureSettingService =
//                new FeatureSettingService<Feature>(availabilityChecker, featureSettingRepo);
//            var manifestCreationStrategy = new ManifestCreationStrategyDefault<Feature>(featureSettingRepo, featureSettingService);
//            var featureManifestService = new FeatureManifestService<Feature>(manifestCreationStrategy);
//            _featureManifest = featureManifestService.GetManifest();
//        }

//        [Test]
//        public void IsAvailable_WhenTheAvailabilityCheckingFunctionReturnsTrueAndDependenciesAreOK_ReturnsTrue()
//        {
//            Assert.That(Feature.TestFeatureE.IsAvailable(_featureManifest));
//        }

//        [Test]
//        public void Setting_WithFullName_RetrievedOK()
//        {
//            Assert.That(Feature.TestFeatureE.Setting(FeatureSettingNames.TestFeatureE.AssemblyName, _featureManifest) == "testFeatureSetting1Value");
//        }

//        [Test]
//        public void Setting_WithoutFullName_RetrievedOK()
//        {
//            Assert.That(Feature.TestFeatureE.Setting(FeatureSettingNames.TestFeatureE.SimpleSetting, _featureManifest) == "testFeatureSetting2Value");
//        }

//        [Test]
//        public void IsAvailable_WhenTheAvailabilityCheckingFunctionReturnsFalse_ReturnsFalse()
//        {
//            Assert.That(!Feature.TestFeatureA.IsAvailable(_featureManifest));
//            Assert.That(!Feature.TestFeatureB.IsAvailable(_featureManifest));
//            Assert.That(!Feature.TestFeatureC.IsAvailable(_featureManifest));
//            Assert.That(!Feature.TestFeatureD.IsAvailable(_featureManifest));
//        }

//        /// <summary>
//        /// A function to test the availability checking behavior.
//        /// </summary>
//        private static bool MyAvailabilityCheckFunction(FeatureSetting<Feature,DefaultTenantEnum> s, EmptyArgs args)
//        {
//            return Enum.GetName(typeof(Feature), s.Feature) == "TestFeatureE";
//        }
//    }
//}
//// ReSharper restore InconsistentNaming