// Copyright 2012, Ben Aston (ben@bj.ma).
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

//The following program takes the example given 
//in the readme.md file on GitHub 
//(https://github.com/benaston/NFeature) and 
//turns it into an executable example.

namespace NFeature.Example.Console
{
	using System;
	using Configuration;
	using DefaultImplementations;
	using Exceptions;

	//0. Get it (ensure the Nuget reference in this solution is up-to-date)

	//1. Define some features
	public enum Feature
	{
		MyFeature,
		MyOtherFeature,
		MyOtherOtherFeature,
	}

	public enum MyFeatureSettings
	{
		mySetting,
	}

	/// <summary>
	/// Demonstration of NFeature basics. Uses the default 
	/// availability check function, and some minimal 
	/// settings in the app.config to demonstrate the 
	/// checking of feature availability and the reading 
	/// of a feature setting.
	/// </summary>
	internal class Program
	{
		private static void Main(string[] a) {
			//2. Define the availability-checking function
			Func<FeatureSetting<Feature, DefaultTenantEnum>, EmptyArgs, bool> fn =
				(f, args) =>
				DefaultFunctions.AvailabilityCheckFunction(f,
				                                           Tuple.Create(FeatureVisibilityMode.Normal,
				                                                        DefaultTenantEnum.All,
				                                                        DateTime.Now));

			//3. Take care of feature manifest initialization
			//NOTE: I suggest hiding this ugly initialization logic away in the IOC container configuration	
			var featureSettingRepo = new AppConfigFeatureSettingRepository<Feature>();
			var availabilityChecker =
				new FeatureSettingAvailabilityChecker<Feature, EmptyArgs, DefaultTenantEnum>(fn);
			//from step 2
			var featureSettingService =
				new FeatureSettingService<Feature, DefaultTenantEnum, EmptyArgs>(availabilityChecker,
				                                                                 featureSettingRepo);
			var manifestCreationStrategy =
				new ManifestCreationStrategyDefault<Feature, DefaultTenantEnum>(featureSettingRepo,
				                                                                featureSettingService);
			//we use the default for this example
			var featureManifestService = new FeatureManifestService<Feature>(manifestCreationStrategy);
			IFeatureManifest<Feature> featureManifest = featureManifestService.GetManifest();

			//4. Configure feature dependencies (see the web.config - we do not specify any dependencies for this demo)

			//5. Add code that is conditional on feature availability. featureManifest ideally supplied via IOC container
			if (Feature.MyFeature.IsAvailable(featureManifest)) {
				Console.WriteLine("MyFeature is available.");
			} else {
				throw new FeatureNotAvailableException(
					"MyFeature is not available. This is unexpected behavior for the default implementation of NFeature.Example.Console.",
					new[] {
						"Check your app.config.", "Ensure built DLLs are up to date.",
						"Ensure you have not modified this application or its configuration."
					});
			}

			//6. Optionally configure feature-specific settings using JSON
			Console.WriteLine(Feature.MyFeature.Setting(MyFeatureSettings.mySetting, featureManifest));
			Console.ReadLine();

			//7. Optionally specify dates for feature availability

			//8. At some future date optionally mark your feature as 
			//Established to indicate that it is now integral to your 
			//application and cannot be turned off (see footnote 2)

			//9. ...

			//10. Profit!
		}
	}
}