// Copyright 2011, Ben Aston (ben@bj.ma.)
// 
// This file is part of NFeature.
// 
// NFeature is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NFeature is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NFeature.  If not, see <http://www.gnu.org/licenses/>.

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
		public void
			IsAvailable_WhenInvokedAgainstAnEstablishedFeature_ThrowsAnException_BecauseEstablishedFeaturesCannotBeAnythingOtherThanEnabled
			()
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
			                      						new Dictionary<string, dynamic>
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
			                      						new Dictionary<string, dynamic>
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
			                      						new Dictionary<string, dynamic>
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