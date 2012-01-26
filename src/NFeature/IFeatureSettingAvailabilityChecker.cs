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

namespace NFeature
{
	using System.Collections.Generic;
	using Configuration;

	public interface IFeatureSettingAvailabilityChecker<TFeatureEnum> :
		IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, EmptyArgs>
		where TFeatureEnum : struct {}

	public interface IFeatureSettingAvailabilityChecker<TFeatureEnum, in TAvailabilityCheckArgs> :
		IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs>
		where TFeatureEnum : struct {}

	public interface IFeatureSettingAvailabilityChecker<TFeatureEnum, TTenant, in TAvailabilityCheckArgs>
		where TFeatureEnum : struct
		where TTenant : struct
	{
		bool RecursivelyCheckAvailability(FeatureSetting<TFeatureEnum, TTenant> featureSettingToCheck,
		                                  FeatureSetting<TFeatureEnum, TTenant>[] allFeatureSettings,
		                                  TAvailabilityCheckArgs availabilityCheckTuple =
		                                  	default(TAvailabilityCheckArgs),
		                                  List<FeatureSetting<TFeatureEnum, TTenant>> featuresCurrentlyUnderAnalysis =
		                                  	null);
	}
}