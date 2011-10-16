// ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System.Collections.Generic;
    using Exceptions;
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class FeatureExtensionsTests
    {
        [Test]
        public void IsAvailable_WhenInvokedAgainstAnEstablishedFeature_ThrowsAnException_BecauseEstablishedFeaturesCannotBeAnythingOtherThanEnabled()
        {
            //arrange
            const string desiredSettingValue = "test@example.com";
            var featureManifest = new FeatureManifest<Feature>
                                      {
                                          {
                                              Feature.TestFeatureA,
                                              new FeatureDescriptor<Feature>(Feature.TestFeatureA)
                                                  {
                                                      IsEstablished = true,
                                                      Settings =
                                                          new Dictionary<string, string>
                                                              {
                                                                  {
                                                                      "ExampleSettingName",
                                                                      desiredSettingValue
                                                                      }
                                                              }
                                                  }
                                              }
                                      };

            //act / assert
            Assert.Throws
                <EstablishedFeatureAvailabilityCheckException<Feature>>
                (() => Feature.TestFeatureA.IsAvailable(featureManifest));
        }

        [Test]
        public void Setting_WhenInvokedAgainstFeatureNotAvailable_ThrowsException()
        {
            //arrange
            var featureManifest = new FeatureManifest<Feature>
                                      {
                                          {
                                              Feature.TestFeatureA,
                                              new FeatureDescriptor<Feature>(Feature.TestFeatureA)
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
            Assert.Throws<FeatureNotAvailableException>(() => Feature.TestFeatureA.Setting(
                    FeatureSettingNames.TestFeature1.ExampleSettingName, featureManifest));
        }

        [Test]
        public void Setting_WhenInvoked_ReturnsTheValueAssociatedWithThatSetting()
        {
            //arrange
            const string desiredSettingValue = "test@example.com";
            var featureManifest = new FeatureManifest<Feature>
                                      {
                                          {
                                              Feature.TestFeatureE,
                                              new FeatureDescriptor<Feature>(Feature.TestFeatureE)
                                                  {
                                                      IsAvailable = true,
                                                      Settings =
                                                          new Dictionary<string, string>
                                                              {
                                                                  {
                                                                      "ExampleSettingName",
                                                                      desiredSettingValue
                                                                      }
                                                              }
                                                  }
                                              }
                                      };

            //act
            var actualSettingValue =
                Feature.TestFeatureE.Setting(
                    FeatureSettingNames.TestFeature1.ExampleSettingName, featureManifest);

            //assert
            Assert.That(actualSettingValue == desiredSettingValue);
        }
    }
}

// ReSharper restore InconsistentNaming