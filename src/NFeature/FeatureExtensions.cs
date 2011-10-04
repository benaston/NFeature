namespace NFeature
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NBasicExtensionMethod;
    using NSure;
    using ArgumentNullException = NHelpfulException.FrameworkExceptions.ArgumentNullException;

    public static class FeatureExtensions
    {
        public static bool IsAvailable<TFeatureEnumeration>(this TFeatureEnumeration feature, IFeatureManifest<TFeatureEnumeration> featureManifest)
            where TFeatureEnumeration :struct
        {
            Ensure.That<ArgumentNullException>(featureManifest.IsNotNull(), "featureManifest not supplied.");

            try
            {
                return featureManifest[feature].IsAvailable;
            }
            catch (KeyNotFoundException e)
            {
                throw new FeatureNotConfiguredException<TFeatureEnumeration>(feature, e);
            }
        }

        public static string Setting<TFeatureEnumeration>(this TFeatureEnumeration feature, Enum settingName, IFeatureManifest<TFeatureEnumeration> featureManifest)
                where TFeatureEnumeration : struct
        {
            Ensure.That<ArgumentNullException>(featureManifest.IsNotNull(), "featureManifest not supplied.")
                .And<FeatureNotAvailableException<TFeatureEnumeration>>(feature.IsAvailable(featureManifest),
                                                   "Specified feature is unavailable.");

            try
            {
                return featureManifest[feature].Settings[Enum.GetName(settingName.GetType(), settingName)];
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to find setting \"{0}\".", settingName), e);
            }
        }

        /// <summary>
        ///   Designed for use for features that the feature subsystem itself depends upon.
        ///   Provides a way of retrieving feature setting information without the 
        ///   FeatureManifest being pre-instantiated.
        /// </summary>
        internal static string Setting<TFeatureEnumeration>(this TFeatureEnumeration feature, Enum settingName,
                                       IFeatureSettingRepository<TFeatureEnumeration> featureSettingRepository)
            where TFeatureEnumeration : struct
        {
            try
            {
                var featureSettings = featureSettingRepository.GetFeatureSettings();
                var featureSetting = featureSettings.Where(s => s.Feature.Equals(feature)).First(); //was ==
                Ensure.That <FeatureConfigurationException<TFeatureEnumeration>>(featureSetting.IsRequiredByFeatureSubsystem,
                                                           "Specified feature not marked as being required by the feature subsystem.");

                return featureSetting.Settings[Enum.GetName(settingName.GetType(), settingName)];
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Unable to find setting \"{0}\".", settingName), e);
            }
        }
    }
}