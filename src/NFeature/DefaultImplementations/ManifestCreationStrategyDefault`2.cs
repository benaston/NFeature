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

namespace NFeature.DefaultImplementations
{
	using Configuration;

	public class ManifestCreationStrategyDefault<TFeatureEnum, TTenantEnum> :
		IFeatureManifestCreationStrategy<TFeatureEnum>
		where TFeatureEnum : struct
		where TTenantEnum : struct
	{
		private readonly IFeatureSettingRepository<TFeatureEnum, TTenantEnum> _featureSettingRepository;
		private readonly IFeatureSettingService<TFeatureEnum, TTenantEnum, EmptyArgs> _featureSettingService;

		public ManifestCreationStrategyDefault(IFeatureSettingRepository<TFeatureEnum, TTenantEnum> featureSettingRepository,
		                                       IFeatureSettingService<TFeatureEnum, TTenantEnum, EmptyArgs>
		                                       	featureSettingService)
		{
			_featureSettingRepository = featureSettingRepository;
			_featureSettingService = featureSettingService;
		}

		public IFeatureManifest<TFeatureEnum> CreateFeatureManifest()
		{
			var featureSettings = _featureSettingRepository.GetFeatureSettings();
			var manifest = new FeatureManifest<TFeatureEnum>();

			foreach (var setting in featureSettings)
			{
				var isAvailable = _featureSettingService
					.AllDependenciesAreSatisfiedForTheFeatureSetting(setting, new EmptyArgs());

				manifest.Add(setting.Feature,
				             new FeatureDescriptor<TFeatureEnum>(setting.Feature)
				             	{
				             		Dependencies = setting.Dependencies,
				             		IsAvailable = isAvailable,
				             		Settings = setting.Settings,
				             	});
			}

			return manifest;
		}
	}
}