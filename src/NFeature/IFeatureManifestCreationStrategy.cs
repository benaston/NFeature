// Copyright 2011, Ben Aston (ben@bj.ma).
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

namespace NFeature
{
	///<summary>
	///	Responsible for defining the interface for types 
	/// that provide functionality for feature manifest 
	/// creation. These strategies might for example, 
	/// take into account cookie configuration, domain 
	/// configuration and user role.
	///</summary>
	///<example>
	///	Suggested implementation is to inject the 
	/// FeatureSettingService and 
	/// FeatureSetttingRepository via the constructor 
	/// along with whatever other services you need to 
	/// determine availability.
	///</example>
	///<code>
	///	<![CDATA[
	/// public IFeatureManifest<TestFeatureList> CreateFeatureManifest()
	/// {
	///     var featureSettings = _featureSettingRepository.GetFeatureSettings();
	///     var manifest = new FeatureManifest<TestFeatureList>();
	///
	///     foreach (var setting in featureSettings)
	///     {
	///         var isAvailable = _featureSettingService
	///             .AllDependenciesAreSatisfiedForTheFeatureSetting(setting, new EmptyArgs());
	///
	///         manifest.Add(setting.Feature,
	///                      new FeatureDescriptor<TestFeatureList>(setting.Feature)
	///                      {
	///                          Dependencies = setting.Dependencies,
	///                          IsAvailable = isAvailable,
	///                          Settings = setting.Settings,
	///                      });
	///     }
	///
	///     return manifest;
	///  }
	/// ]]>
	///</code>
	public interface IFeatureManifestCreationStrategy<TFeatureEnum>
		where TFeatureEnum : struct
	{
		IFeatureManifest<TFeatureEnum> CreateFeatureManifest();
	}
}