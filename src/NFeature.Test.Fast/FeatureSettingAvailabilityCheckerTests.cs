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
        public void CheckAvailability_AIsEstablishedBIsNot_CheckAvailabilityThrowsException()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.Throws<EstablishedFeatureDependencyException<TestFeatureList>>(() => dependencyChecker.CheckAvailability(featureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndAAndBStartDatesAreInPast_CheckAvailability()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(dependencyChecker.CheckAvailability(featureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void
            CheckAvailability_ADependsOnBAndAStartDateIsInFuture_DependenciesAreNotMet_BecauseAIsNotYetAvailable()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(!dependencyChecker.CheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnA_CircularDependencyExceptionIsThrown()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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
                () => dependencyChecker.CheckAvailability(featureSetting,
                                                                    allFeatureSettings,
                                                                    FeatureVisibilityMode.Normal, Tenant.All,
                                                                    DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndAIsNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(!dependencyChecker.CheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndAllEnabled_CheckAvailability()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(dependencyChecker.CheckAvailability(featureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndBAndCAreNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(!dependencyChecker.CheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndBIsNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(!dependencyChecker.CheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBDependsOnCAndCIsNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(!dependencyChecker.CheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void
            CheckAvailability_ADependsOnBAndBIsPreviewableAndModeIsNormal_DependenciesAreNotMet_BecauseModeShouldBePreview
            ()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(!dependencyChecker.CheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void
            CheckAvailability_ADependsOnBAndBStartDateIsInFuture_DependenciesAreNotMet_BecauseBIsNotYetAvailable()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(!dependencyChecker.CheckAvailability(featureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBStartDateIsInPast_CheckAvailability()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(dependencyChecker.CheckAvailability(featureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndBothEnabled_CheckAvailability()
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
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

            Assert.That(dependencyChecker.CheckAvailability(featureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void CheckAvailability_ADependsOnBAndDAndBDependsOnCAndAllAreEnabled_CheckAvailability()
            //a single feature can have multiple dependencies
        {
            var dependencyChecker = new FeatureSettingAvailabilityChecker<TestFeatureList>();
            var allFeatureSettings = new[]
                                         {
                                             new FeatureSetting<TestFeatureList> //A
                                                 {
                                                     Feature = TestFeatureList.TestFeature3,
                                                     Dependencies =
                                                         new[]
                                                             {
                                                                 TestFeatureList.TestFeature1, TestFeatureList.TestFeature2
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

            Assert.That(dependencyChecker.CheckAvailability(featureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }
    }
}

// ReSharper restore InconsistentNaming