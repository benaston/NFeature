// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System;
    using NBasicExtensionMethod;
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class FeatureSettingAvailabilityCheckerTests
    {
        [Test]
        public void CheckAvailability_ADependsOnBAndAAndBStartDatesAreInPast_CheckAvailability()
        {
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Hence(),
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies = new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new[] {TestFeatureList.TestFeature3},
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies = new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new[] {TestFeatureList.TestFeature4},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //C
                                                 {
                                                     Feature = TestFeatureList.TestFeature4,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies = new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new[] {TestFeatureList.TestFeature4},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //C
                                                 {
                                                     Feature = TestFeatureList.TestFeature4,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies = new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new[] {TestFeatureList.TestFeature4},
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //C
                                                 {
                                                     Feature = TestFeatureList.TestFeature4,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies = new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new[] {TestFeatureList.TestFeature4},
                                                     FeatureState = FeatureState.Disabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //C
                                                 {
                                                     Feature = TestFeatureList.TestFeature4,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies = new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new[] {TestFeatureList.TestFeature4},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //C
                                                 {
                                                     Feature = TestFeatureList.TestFeature4,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies = new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[]
                                                             {
                                                                 TestFeatureList.TestFeature1,
                                                                 TestFeatureList.TestFeature2
                                                             },
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new[] {TestFeatureList.TestFeature4},
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //C
                                                 {
                                                     Feature = TestFeatureList.TestFeature4,
                                                     Dependencies = new TestFeatureList[0],
                                                     FeatureState = FeatureState.Enabled,
                                                 },
                                             new FeatureSetting<TestFeatureList> //D
                                                 {
                                                     Feature = TestFeatureList.TestFeature2,
                                                     Dependencies = new TestFeatureList[0],
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
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[] {TestFeatureList.TestFeature1},
                                                     FeatureState = FeatureState.Established,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                             new FeatureSetting<TestFeatureList> //B
                                                 {
                                                     Feature = TestFeatureList.TestFeature1,
                                                     Dependencies = new TestFeatureList[0],
                                                     FeatureState = FeatureState.Enabled,
                                                     StartDtg = 1.Day().Ago(),
                                                 },
                                         };
            var featureSetting = allFeatureSettings[0];

            Assert.Throws<EstablishedFeatureDependencyException<TestFeatureList>>(
                () => _dependencyChecker.RecursivelyCheckAvailability(featureSetting,
                                                                     allFeatureSettings,
                                                                     new Tuple<FeatureVisibilityMode, Tenant, DateTime>(FeatureVisibilityMode.Normal, Tenant.All, DateTime.Now)
                                                                     ));
        }

        private readonly FeatureSettingAvailabilityChecker<TestFeatureList, Tuple<FeatureVisibilityMode, Tenant, DateTime>> _dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList,
                                                                          Tuple<FeatureVisibilityMode, Tenant, DateTime>>
                                        ((s, args) => s.IsAvailable(args.Item1, args.Item2, args.Item3));
    }
}

// ReSharper restore InconsistentNaming