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
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Configuration;
	using Exceptions;
	using NBasicExtensionMethod;
	using NSure;
	using ArgumentNullException = NHelpfulException.FrameworkExceptions.ArgumentNullException;

	/// <remarks>
	/// 	NOTE 1: BA; we maintain a list of features we have met 
	/// 	and not yet resolved the dependencies for. If we come 
	/// 	across a feature and find it in the 
	/// 	featuresUnderAnalysis list then we realise that 
	/// 	resolution of a feature's dependency graph depends on 
	/// 	resolution of it's graph and hence we cannot complete, 
	/// 	and we throw an exception. 
	/// 	NOTE 2: BA; the dependencies of established features 
	/// 		must themselves all be established. 
	/// 	NOTE 3: BA; required due to recursive invocation.
	/// </remarks>
	public class FeatureSettingAvailabilityChecker<TFeatureEnum, TAvailabilityCheckArgs, TTenant> :
		IFeatureSettingAvailabilityChecker<TFeatureEnum, TTenant, TAvailabilityCheckArgs>
		where TFeatureEnum : struct
		where TTenant : struct
	{
		protected Func<FeatureSetting<TFeatureEnum, TTenant>, TAvailabilityCheckArgs, bool>
			AvailabilityCheckFunction;

		public FeatureSettingAvailabilityChecker(
			Func<FeatureSetting<TFeatureEnum, TTenant>, TAvailabilityCheckArgs, bool>
				availabilityCheckFunction) {
			AvailabilityCheckFunction = availabilityCheckFunction;
		}

		/// <summary>
		/// 	Responsible for checking whether the dependencies 
		/// 	for a feature are met. TODO: review use of tuple 
		/// 	for params for custom availability checking 
		/// 	fucntionality.
		/// </summary>
		public bool RecursivelyCheckAvailability(
			FeatureSetting<TFeatureEnum, TTenant> featureSettingToCheck,
			FeatureSetting<TFeatureEnum, TTenant>[] allFeatureSettings,
			TAvailabilityCheckArgs availabilityCheckArgs =
				default(TAvailabilityCheckArgs),
			List<FeatureSetting<TFeatureEnum, TTenant>>
				featuresCurrentlyUnderAnalysis = null) {
			Ensure.That<ArgumentNullException>(featureSettingToCheck.IsNotNull(),
			                                   "featureSetting not supplied.")
				.And<ArgumentNullException>(allFeatureSettings.IsNotNull(),
				                            "allFeatureSettings not supplied.");

			featuresCurrentlyUnderAnalysis = featuresCurrentlyUnderAnalysis ??
			                                 new List<FeatureSetting<TFeatureEnum, TTenant>>();
			if (featuresCurrentlyUnderAnalysis.Contains(featureSettingToCheck)) //see note 1
			{
				throw new CircularFeatureSettingDependencyException();
			}

			featuresCurrentlyUnderAnalysis.Add(featureSettingToCheck);

			foreach (TFeatureEnum dependency in featureSettingToCheck.Dependencies) {
				try {
					TFeatureEnum dependencyClosedOver = dependency;
					FeatureSetting<TFeatureEnum, TTenant> dependencySetting =
						allFeatureSettings.First(s => s.Feature.Equals(dependencyClosedOver));

					if (featureSettingToCheck.FeatureState == FeatureState.Established
					    && dependencySetting.FeatureState != FeatureState.Established) //see note 2
					{
						throw new EstablishedFeatureDependencyException<TFeatureEnum>(
							featureSettingToCheck.Feature, dependencyClosedOver);
					}

					if (!RecursivelyCheckAvailability(dependencySetting,
					                                  allFeatureSettings,
					                                  availabilityCheckArgs,
					                                  featuresCurrentlyUnderAnalysis)) {
						return false;
					}
				} catch (InvalidOperationException e) {
					throw new FeatureNotConfiguredException<TFeatureEnum>(dependency, e);
				}
			}

			featuresCurrentlyUnderAnalysis.Remove(featureSettingToCheck); //see note 3

			return AvailabilityCheckFunction(featureSettingToCheck, availabilityCheckArgs);
		}
	}
}