// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using NFeature;

    [TestFixture]
    [Category("Slow")]
    public class FeatureExtensionsTests
    {
        [Test]
        public void Setting_WhenInvoked_ReturnsTheValueAssociatedWithThatSetting()
        {
            //arrange
            const string desiredSettingValue = "test@example.com";
            var featureManifest = new FeatureManifest<TestFeatureList>
                                      {
                                          {
                                              TestFeatureList.TestFeature5,
                                              new FeatureDescriptor<TestFeatureList>
                                                  {
                                                      Settings =
                                                          new Dictionary<string, string>
                                                              {{"JustGivingEmailRecipientEmailAddress", desiredSettingValue}}
                                                  }
                                              }
                                      };

            //act
            var actualSettingValue =
                TestFeatureList.TestFeature5.Setting(FeatureSettingNames.CharitySelfSignUpEmail.JustGivingEmailRecipientEmailAddress, featureManifest);

            //assert
            Assert.That(actualSettingValue == desiredSettingValue);
        }
    }
}
// ReSharper restore InconsistentNaming