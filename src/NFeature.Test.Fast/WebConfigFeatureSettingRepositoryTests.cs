// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    [Category("Slow")]
    public class WebConfigFeatureSettingRepositoryTests
    {
        [Test]
        public void GetFeatureSettings_WhenInvoked_FeatureSettingsWithNoSpecifiedDomainAreAvailableToAllDomains()
        {
            var r = new WebConfigFeatureSettingRepository<TestFeatureList>();
            var settings = r.GetFeatureSettings();

            Assert.That(
                settings.Where(i => i.Feature == TestFeatureList.TestFeature4).First().SupportedTenants.Contains(
                    Tenant.All));
        }

        [Test]
        public void GetFeatureSettings_WhenInvoked_FeatureSettingsWithSpecifiedDomainsAreAvailabelToThoseDomainsOnly()
        {
            var r = new WebConfigFeatureSettingRepository<TestFeatureList>();
            var settings = r.GetFeatureSettings();

            Assert.That(
                settings.Where(i => i.Feature == TestFeatureList.TestFeature1).First().SupportedTenants.Length == 1);
            Assert.That(
                settings.Where(i => i.Feature == TestFeatureList.TestFeature1).First().SupportedTenants.Contains(
                    Tenant.Tenant1));
        }

        [Test]
        public void GetFeatureSettings_WhenInvoked_ReturnsAllCorrectNumberOfFeatureSettings()
        {
            var r = new WebConfigFeatureSettingRepository<TestFeatureList>();
            var settings = r.GetFeatureSettings();

            Assert.That(settings.Count() == 4);
        }
    }
}

// ReSharper restore InconsistentNaming