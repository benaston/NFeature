// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System;
    using Exceptions;
    using NBasicExtensionMethod;
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class FeatureSettingAvailabilityCheckerTests
    {
        [Test]
        public void RecursivelyCheckAvailability_ADependsOnBAndAAndBStartDatesAreInPast_ReturnTrue()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant>
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies =
                                                         new[] {Feature.TestFeatureB},
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                             new FeatureSetting<Feature, Tenant>
                                                 {
                                                     Feature = Feature.TestFeatureB,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void
            CheckAvailability_ADependsOnBAndAStartDateIsInFuture_DependenciesAreNotMet_BecauseAIsNotYetAvailable()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies =
                                                         new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Hence(),
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                        allFeatureSettings,
                                                                        new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnA_CircularDependencyExceptionIsThrown()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies = new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new[] {Feature.TestFeatureC},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.Throws<CircularFeatureSettingDependencyException>(
                () => _dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                     allFeatureSettings,
                                                                     new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndAIsNotEnabled_DependenciesAreNotMet()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies = new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new[] {Feature.TestFeatureD},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //C
                                                 {
                                                     Feature = Feature.TestFeatureD,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                        allFeatureSettings,
                                                                        new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndAllEnabled_CheckAvailability()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies = new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new[] {Feature.TestFeatureD},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //C
                                                 {
                                                     Feature = Feature.TestFeatureD,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndBAndCAreNotEnabled_DependenciesAreNotMet()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies = new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new[] {Feature.TestFeatureD},
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //C
                                                 {
                                                     Feature = Feature.TestFeatureD,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                        allFeatureSettings,
                                                                        new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndBIsNotEnabled_DependenciesAreNotMet()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies = new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new[] {Feature.TestFeatureD},
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //C
                                                 {
                                                     Feature = Feature.TestFeatureD,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                        allFeatureSettings,
                                                                        new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndCIsNotEnabled_DependenciesAreNotMet()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies = new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new[] {Feature.TestFeatureD},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //C
                                                 {
                                                     Feature = Feature.TestFeatureD,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                        allFeatureSettings,
                                                                        new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void
            CheckAvailability_ADependsOnBAndBIsPreviewableAndModeIsNormal_DependenciesAreNotMet_BecauseModeShouldBePreview
            ()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies =
                                                         new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Previewable,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                        allFeatureSettings,
                                                                        new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void
            CheckAvailability_ADependsOnBAndBStartDateIsInFuture_DependenciesAreNotMet_BecauseBIsNotYetAvailable()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies =
                                                         new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Hence(),
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                        allFeatureSettings,
                                                                        new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBStartDateIsInPast_CheckAvailability()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies =
                                                         new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBothEnabled_CheckAvailability()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies = new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndDAndBDependsOnCAndAllAreEnabled_CheckAvailability()
            //a single feature can have multiple dependencies
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies =
                                                         new[]
                                                             {
                                                                 Feature.TestFeatureA,
                                                                 Feature.TestFeatureB
                                                             },
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new[] {Feature.TestFeatureD},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //C
                                                 {
                                                     Feature = Feature.TestFeatureD,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<Feature, Tenant> //D
                                                 {
                                                     Feature = Feature.TestFeatureB,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)));
        }

        [Test]
        public void CheckAvailability_AIsEstablishedBIsNot_CheckAvailabilityThrowsException()
        {
            
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<Feature, Tenant> //A
                                                 {
                                                     Feature = Feature.TestFeatureC,
                                                     Dependencies =
                                                         new[] {Feature.TestFeatureA},
                                                     FeatureState = FeatureState.Established,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                             new FeatureSetting<Feature, Tenant> //B
                                                 {
                                                     Feature = Feature.TestFeatureA,
                                                     Dependencies = new Feature[0],
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.Throws<EstablishedFeatureDependencyException<Feature>>(
                () => _dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                     allFeatureSettings,
                                                                     new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)
                                                                     ));
        }

        private readonly FeatureSettingAvailabilityChecker<Feature, Tuple<FeatureVisibilityMode, Tenant, DateTime>, Tenant> _dependencyChecker = new FeatureSettingAvailabilityChecker<Feature,
                                                                          Tuple<FeatureVisibilityMode, Tenant, DateTime>, Tenant>
                                        ((s, args) => s.IsAvailable(args.Item1, args.Item2, args.Item3));
    }
}

// ReSharper restore InconsistentNaming