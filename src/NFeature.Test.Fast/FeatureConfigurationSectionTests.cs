 // ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System;
    using System.Linq;
    using NFeature;
    using NUnit.Framework;

    [TestFixture]
    [Category("Slow")]
    public class FeatureConfigurationSectionTests
    {
        private readonly DateTime testStartDtg = new DateTime(1981, 3, 23, 18, 0, 1);
        private readonly DateTime testEndDtg = new DateTime(2081, 3, 23, 18, 0, 1);

        [Test]
        public void FeatureConfigurationSection_Retrieves_Correct_Dependencies()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[0].Dependencies.Length == 2);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_Correct_EndDtg()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[3].EndDtg == testEndDtg);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_Correct_EndDtg_When_None_Specified()
        {
            var s = ConfigurationManager < FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[2].EndDtg == DateTime.MinValue);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_Correct_Number_Of_SupportedTenants()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[0].SupportedTenants.Length == 1);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_Correct_StartDtg()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[2].StartDtg == testStartDtg);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_Correct_SupportedTenants()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[0].SupportedTenants[0] == Tenant.Tenant1);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_FeatureState_OK()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[0].State == FeatureState.Enabled);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_MultipleJsonSettings_OK()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[1].Settings.Count == 2);
            Assert.That(f[1].Settings["testFeatureSetting1"] == "testFeatureSetting1Value");
            Assert.That(f[1].Settings["testFeatureSetting2"] == "testFeatureSetting2Value");
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_Multiple_Features_OK()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f.Count == 4);
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_Name_OK()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[0].Name == "TestFeature1");
        }

        [Test]
        public void FeatureConfigurationSection_Retrieves_SingleJsonSetting_OK()
        {
            var s = ConfigurationManager<FeatureConfigurationSection<TestFeatureList>>.Section();
            var f = s.FeatureSettings;
            Assert.That(f[0].Settings.Count, Is.GreaterThan(0));
            Assert.That(f[0].Settings.First().Key == "testFeatureSetting1");
            Assert.That(f[0].Settings.First().Value == "testFeatureSetting1Value");
        }
    }
}

// ReSharper restore InconsistentNaming