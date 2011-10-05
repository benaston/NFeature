// ReSharper disable InconsistentNaming
namespace NFeature.Test.Slow
{
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    [Category("Slow")]
    public class WebConfigFeatureSettingRepositoryTests
    {
        [Test]
        public void GetFeatureSettings_WhenInvoked_FeatureSettingsAreMarkedAsBeingEstablishedCorrectly()
        {
            var r = new WebConfigFeatureSettingRepository<TestFeatureList>();
            var settings = r.GetFeatureSettings();

            Assert.That(
                settings.Where(i => i.Feature == TestFeatureList.TestFeature5).First().FeatureState == FeatureState.Established );
        }

        [Test]
        public void GetFeatureSettings_WhenInvoked_FeatureSettingsWithNoSpecifiedTenantAreAvailableToAllTenant()
        {
            var r = new WebConfigFeatureSettingRepository<TestFeatureList>();
            var settings = r.GetFeatureSettings();

            Assert.That(
                settings.Where(i => i.Feature == TestFeatureList.TestFeature4).First().SupportedTenants.Contains(
                    Tenant.All));
        }

        [Test]
        public void GetFeatureSettings_WhenInvoked_FeatureSettingsWithSpecifiedTenantsAreAvailableToThoseTenantsOnly()
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

            Assert.That(settings.Count() == 5);
        }
    }
}

// ReSharper restore InconsistentNaming