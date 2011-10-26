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
    ///   NOTE 1: BA; we maintain a list of features we have met and not
    ///   yet resolved the dependencies for. If we come across 
    ///   a feature and find it in the featuresUnderAnalysis
    ///   list then we realise that resolution of a feature's
    ///   dependency graph depends on resolution of it's graph 
    ///   and hence we cannot complete, and we throw an exception.
    ///   NOTE 2: BA; the dependencies of established features must 
    ///   themselves all be established.
    ///   NOTE 3: BA; required due to recursive invocation.
    /// </remarks>
    public class FeatureSettingAvailabilityChecker<TFeatureEnum, TAvailabilityCheckArgs, TTenant> :
        IFeatureSettingAvailabilityChecker<TFeatureEnum, TTenant, TAvailabilityCheckArgs>
        where TFeatureEnum : struct
        where TTenant : struct
    {
        protected Func<FeatureSetting<TFeatureEnum, TTenant>, TAvailabilityCheckArgs, bool>
            AvailabilityCheckFunction;

        public FeatureSettingAvailabilityChecker(
            Func<FeatureSetting<TFeatureEnum, TTenant>, TAvailabilityCheckArgs, bool> availabilityCheckFunction)
        {
            AvailabilityCheckFunction = availabilityCheckFunction;
        }

        /// <summary>
        ///   Responsible for checking whether the dependencies 
        ///   for a feature are met.
        ///   TODO: review use of tuple for params for custom availability checking fucntionality.
        /// </summary>
        public bool RecursivelyCheckAvailability(FeatureSetting<TFeatureEnum, TTenant> featureSettingToCheck,
                                                 FeatureSetting<TFeatureEnum, TTenant>[] allFeatureSettings,
                                                 TAvailabilityCheckArgs availabilityCheckArgs =
                                                     default(TAvailabilityCheckArgs),
                                                 List<FeatureSetting<TFeatureEnum, TTenant>>
                                                     featuresCurrentlyUnderAnalysis = null)
        {
            Ensure.That<ArgumentNullException>(featureSettingToCheck.IsNotNull(), "featureSetting not supplied.")
                .And<ArgumentNullException>(allFeatureSettings.IsNotNull(), "allFeatureSettings not supplied.");

            featuresCurrentlyUnderAnalysis = featuresCurrentlyUnderAnalysis ??
                                             new List<FeatureSetting<TFeatureEnum, TTenant>>();
            if (featuresCurrentlyUnderAnalysis.Contains(featureSettingToCheck)) //see note 1
            {
                throw new CircularFeatureSettingDependencyException();
            }

            featuresCurrentlyUnderAnalysis.Add(featureSettingToCheck);

            foreach (var dependency in featureSettingToCheck.Dependencies)
            {
                try
                {
                    var dependencyClosedOver = dependency;
                    var dependencySetting = allFeatureSettings.First(s => s.Feature.Equals(dependencyClosedOver));

                    if (featureSettingToCheck.FeatureState == FeatureState.Established
                        && dependencySetting.FeatureState != FeatureState.Established) //see note 2
                    {
                        throw new EstablishedFeatureDependencyException<TFeatureEnum>(
                            featureSettingToCheck.Feature, dependencyClosedOver);
                    }

                    if (!RecursivelyCheckAvailability(dependencySetting,
                                                      allFeatureSettings,
                                                      availabilityCheckArgs,
                                                      featuresCurrentlyUnderAnalysis))
                    {
                        return false;
                    }
                }
                catch (InvalidOperationException e)
                {
                    throw new FeatureNotConfiguredException<TFeatureEnum>(dependency, e);
                }
            }

            featuresCurrentlyUnderAnalysis.Remove(featureSettingToCheck); //see note 3

            return AvailabilityCheckFunction(featureSettingToCheck, availabilityCheckArgs);
        }
    }
}