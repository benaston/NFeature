// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class FeatureExtensionsTests
    {
        [Test]
        public void IsAvailable_WhenInvokedAgainstAnEstablishedFeature_ThrowsAnException()
        {
            //arrange
            const string desiredSettingValue = "test@example.com";
            var featureManifest = new FeatureManifest<TestFeatureList>
                                      {
                                          {
                                              TestFeatureList.TestFeature1,
                                              new FeatureDescriptor<TestFeatureList>(TestFeatureList.TestFeature1)
                                                  {
                                                      IsEstablished = true,
                                                      Settings =
                                                          new Dictionary<string, string>
                                                              {
                                                                  {
                                                                      "JustGivingEmailRecipientEmailAddress",
                                                                      desiredSettingValue
                                                                      }
                                                              }
                                                  }
                                              }
                                      };

            //act / assert
            Assert.Throws
                <EstablishedFeatureAvailabilityCheckException<TestFeatureList>>
                (() => TestFeatureList.TestFeature1.IsAvailable(featureManifest));
        }

        [Test]
        public void Setting_WhenInvokedAgainstFeatureNotAvailable_ThrowsException()
        {
            //arrange
            var featureManifest = new FeatureManifest<TestFeatureList>
                                      {
                                          {
                                              TestFeatureList.TestFeature1,
                                              new FeatureDescriptor<TestFeatureList>(TestFeatureList.TestFeature1)
                                                  {
                                                      IsAvailable = false,
                                                      Settings =
                                                          new Dictionary<string, string>
                                                              {
                                                                  {
                                                                      "SettingName",
                                                                      "SettingValue"
                                                                      }
                                                              }
                                                  }
                                              }
                                      };


            //assert
            Assert.Throws<FeatureNotAvailableException>(() => TestFeatureList.TestFeature1.Setting(
                    FeatureSettingNames.CharitySelfSignUpEmail.JustGivingEmailRecipientEmailAddress, featureManifest));
        }

        [Test]
        public void Setting_WhenInvoked_ReturnsTheValueAssociatedWithThatSetting()
        {
            //arrange
            const string desiredSettingValue = "test@example.com";
            var featureManifest = new FeatureManifest<TestFeatureList>
                                      {
                                          {
                                              TestFeatureList.TestFeature5,
                                              new FeatureDescriptor<TestFeatureList>(TestFeatureList.TestFeature5)
                                                  {
                                                      IsAvailable = true,
                                                      Settings =
                                                          new Dictionary<string, string>
                                                              {
                                                                  {
                                                                      "JustGivingEmailRecipientEmailAddress",
                                                                      desiredSettingValue
                                                                      }
                                                              }
                                                  }
                                              }
                                      };

            //act
            var actualSettingValue =
                TestFeatureList.TestFeature5.Setting(
                    FeatureSettingNames.CharitySelfSignUpEmail.JustGivingEmailRecipientEmailAddress, featureManifest);

            //assert
            Assert.That(actualSettingValue == desiredSettingValue);
        }
    }
}

// ReSharper restore InconsistentNaming