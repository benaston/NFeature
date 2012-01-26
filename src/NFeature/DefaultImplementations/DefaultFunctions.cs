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

namespace NFeature.DefaultImplementations
{
	using System;
	using System.Linq;
	using Configuration;

	public static class DefaultFunctions
	{
		public static bool AvailabilityCheckFunction<TFeatureEnum>(
			FeatureSetting<TFeatureEnum, DefaultTenantEnum> s,
			Tuple<FeatureVisibilityMode, DefaultTenantEnum, DateTime> args)
			where TFeatureEnum : struct
		{
			return ((s.FeatureState == FeatureState.Enabled ||
			         (s.FeatureState == FeatureState.Previewable && args.Item1 == FeatureVisibilityMode.Preview)) &&
			        s.StartDtg <= args.Item3 &&
			        s.EndDtg > args.Item3);
		}

		/// <summary>
		/// 	The default imeplementation of the availability checker. Checks the tenancy, feature state and date/time. Can of course be substituted for by your own function. For example, your own function might take into consideration a number indicating site load or user role.
		/// </summary>
		/// <example>
		/// 	<![CDATA[
		/// return 
		/// (s.FeatureState == FeatureState.Enabled ||
		/// (s.FeatureState == FeatureState.Previewable && args.Item1 == FeatureVisibilityMode.Preview)) &&
		/// s.StartDtg <= args.Item3 &&
		/// s.EndDtg > args.Item3;
		/// ]]>
		/// </example>
		public static bool AvailabilityCheckFunction<TFeatureEnum, TTenantEnum>(
			FeatureSetting<TFeatureEnum, TTenantEnum> s,
			Tuple<FeatureVisibilityMode, TTenantEnum, DateTime> args)
			where TFeatureEnum : struct
			where TTenantEnum : struct
		{
			return (s.SupportedTenants.Contains((TTenantEnum) Enum.ToObject(typeof (TTenantEnum), 0)) ||
			        s.SupportedTenants.Contains(args.Item2)) &&
			       (s.FeatureState == FeatureState.Enabled ||
			        (s.FeatureState == FeatureState.Previewable && args.Item1 == FeatureVisibilityMode.Preview)) &&
			       s.StartDtg <= args.Item3 &&
			       s.EndDtg > args.Item3;
		}
	}
}