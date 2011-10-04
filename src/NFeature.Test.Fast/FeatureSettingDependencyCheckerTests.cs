// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System;
    using NBasicExtensionMethod;
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class FeatureSettingDependencyCheckerTests
    {
        [Test]
        public void DependenciesAreMet_ADependsOnBAndAAndBStartDatesAreInPast_DependenciesAreMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void
            DependenciesAreMet_ADependsOnBAndAStartDateIsInFuture_DependenciesAreNotMet_BecauseAIsNotYetAvailable()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(!dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBDependsOnA_CircularDependencyExceptionIsThrown()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.Throws<CircularFeatureSettingDependencyException>(
                () => dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                    allFeatureSettings,
                                                                    FeatureVisibilityMode.Normal, Tenant.All,
                                                                    DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBDependsOnCAndAIsNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(!dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBDependsOnCAndAllEnabled_DependenciesAreMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBDependsOnCAndBAndCAreNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(!dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBDependsOnCAndBIsNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(!dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBDependsOnCAndCIsNotEnabled_DependenciesAreNotMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(!dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void
            DependenciesAreMet_ADependsOnBAndBIsPreviewableAndModeIsNormal_DependenciesAreNotMet_BecauseModeShouldBePreview
            ()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(!dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void
            DependenciesAreMet_ADependsOnBAndBStartDateIsInFuture_DependenciesAreNotMet_BecauseBIsNotYetAvailable()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(!dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                       allFeatureSettings,
                                                                       FeatureVisibilityMode.Normal, Tenant.All,
                                                                       DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBStartDateIsInPast_DependenciesAreMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndBothEnabled_DependenciesAreMet()
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }

        [Test]
        public void DependenciesAreMet_ADependsOnBAndDAndBDependsOnCAndAllAreEnabled_DependenciesAreMet()
            //a single feature can have multiple dependencies
        {
            var dependencyChecker = new FeatureSettingDependencyChecker<TestFeatureList>();
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
            var furnFeatureSetting = allFeatureSettings[0];

            Assert.That(dependencyChecker.AreDependenciesMetForTenant(furnFeatureSetting,
                                                                      allFeatureSettings,
                                                                      FeatureVisibilityMode.Normal, Tenant.All,
                                                                      DateTime.Now));
        }
    }
}

// ReSharper restore InconsistentNaming