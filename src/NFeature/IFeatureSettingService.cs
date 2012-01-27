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
	using Configuration;

	public interface IFeatureSettingService<TFeatureEnum> :
		IFeatureSettingService<TFeatureEnum, DefaultTenantEnum, EmptyArgs>
		where TFeatureEnum : struct {}

	/// <summary>
	/// 	Responsible for encapsulating functionality related to FeatureSetting that makes more sense to be placed on a service type.
	/// </summary>
	public interface IFeatureSettingService<TFeatureEnum, TTenantEnum, in TAvailabilityCheckArgs>
		where TFeatureEnum : struct
		where TTenantEnum : struct
	{
		/// <summary>
		/// 	Determines whether the dependencies are satisfied for the specified feature setting.
		/// </summary>
		bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnum, TTenantEnum> f,
		                                                     TAvailabilityCheckArgs availabilityCheckArgs);
	}
}