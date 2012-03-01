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

namespace NFeature
{
	using Configuration;

	public class FeatureSettingService<TFeatureEnum, TAvailabilityCheckArgs> :
		IFeatureSettingService<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs>
		where TFeatureEnum : struct
	{
		private readonly
			IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs>
			_featureSettingAvailabilityChecker;

		private readonly IFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum>
			_featureSettingRepository;

		public FeatureSettingService(
			IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs>
				featureSettingAvailabilityChecker,
			IFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum> featureSettingRepository) {
			_featureSettingAvailabilityChecker = featureSettingAvailabilityChecker;
			_featureSettingRepository = featureSettingRepository;
		}

		public bool AllDependenciesAreSatisfiedForTheFeatureSetting(
			FeatureSetting<TFeatureEnum, DefaultTenantEnum> f,
			TAvailabilityCheckArgs availabilityCheckArgs) {
			return _featureSettingAvailabilityChecker.RecursivelyCheckAvailability(f,
			                                                                       _featureSettingRepository
			                                                                       	.GetFeatureSettings(),
			                                                                       availabilityCheckArgs);
		}
	}
}