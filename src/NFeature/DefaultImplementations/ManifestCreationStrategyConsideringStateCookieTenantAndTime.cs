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
	using System;
	using System.Web;
	using Configuration;
	using NBasicExtensionMethod;

	/// <summary>
	/// 	Constructs the feature manifest according to the tenancy context, the existence of the preview cookie and feature configuration. This provides an example manifest creation strategy, and is replaceable (being a strategy).
	/// </summary>
	public class ManifestCreationStrategyConsideringStateCookieTenantAndTime<TFeatureEnum, TTenantEnum> :
		IFeatureManifestCreationStrategy<TFeatureEnum>
		where TFeatureEnum : struct
		where TTenantEnum : struct
	{
		public const string FeaturePreviewCookieName = "FeaturePreviewCookie";
		private readonly IApplicationClock _clock;

		private readonly
			IFeatureSettingService<TFeatureEnum, TTenantEnum, Tuple<FeatureVisibilityMode, TTenantEnum, DateTime>>
			_featureSettingService;

		private readonly IFeatureSettingRepository<TFeatureEnum, TTenantEnum> _featureSettingsRepository;
		private readonly HttpContextBase _httpContext;
		private readonly ITenancyContext<TTenantEnum> _tenancyContext;

		public ManifestCreationStrategyConsideringStateCookieTenantAndTime(
			IFeatureSettingService<TFeatureEnum, TTenantEnum, Tuple<FeatureVisibilityMode, TTenantEnum, DateTime>>
				featureSettingService,
			IFeatureSettingRepository<TFeatureEnum, TTenantEnum> featureSettingsRepository,
			HttpContextBase httpContext,
			ITenancyContext<TTenantEnum> tenancyContext,
			IApplicationClock clock)
		{
			_featureSettingService = featureSettingService;
			_featureSettingsRepository = featureSettingsRepository;
			_httpContext = httpContext;
			_tenancyContext = tenancyContext;
			_clock = clock;
		}

		public IFeatureManifest<TFeatureEnum> CreateFeatureManifest()
		{
			var featureSettings = _featureSettingsRepository.GetFeatureSettings();
			var manifest = new FeatureManifest<TFeatureEnum>();

			foreach (var setting in featureSettings)
			{
				var featureVisibilityMode = _httpContext.Request.Cookies[FeaturePreviewCookieName].IsNotNull()
				                            	? FeatureVisibilityMode.Preview
				                            	: FeatureVisibilityMode.Normal;

				var isAvailable = _featureSettingService
					.AllDependenciesAreSatisfiedForTheFeatureSetting(setting,
					                                                 new Tuple<FeatureVisibilityMode, TTenantEnum, DateTime>(
					                                                 	featureVisibilityMode,
					                                                 	_tenancyContext.CurrentTenant,
					                                                 	_clock.Now));

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