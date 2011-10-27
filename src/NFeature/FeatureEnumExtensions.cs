namespace NFeature
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Configuration.Exceptions;
    using Exceptions;
    using NBasicExtensionMethod;
    using NSure;
    using ArgumentNullException = NHelpfulException.FrameworkExceptions.ArgumentNullException;

    public static class FeatureEnumExtensions
    {
        public static bool IsAvailable<TFeatureEnum>(this TFeatureEnum feature, IFeatureManifest<TFeatureEnum> featureManifest)
            where TFeatureEnum :struct
        {
            Ensure.That<ArgumentNullException>(featureManifest.IsNotNull(), "featureManifest not supplied.");

            try
            {
                return featureManifest[feature].IsAvailable;
            }
            catch (KeyNotFoundException e)
            {
                throw new FeatureNotConfiguredException<TFeatureEnum>(feature, e);
            }
        }

        public static dynamic Setting<TFeatureEnum>(this TFeatureEnum feature, Enum settingName, IFeatureManifest<TFeatureEnum> featureManifest)
                where TFeatureEnum : struct
        {
            Ensure.That<ArgumentNullException>(featureManifest.IsNotNull(), "featureManifest not supplied.")
                .And<FeatureNotAvailableException>(feature.IsAvailable(featureManifest),
                                                   string.Format("Specified feature '{0}' is unavailable.", Enum.GetName(typeof(TFeatureEnum), feature)));

            try
            {
                //todo: refactor
                var enumItemName = Enum.GetName(settingName.GetType(), settingName);
                var enumItemMember = settingName.GetType().GetField(enumItemName); 
                string enumItemFullName = null;

                if (enumItemMember != null)
                {
                    var attribute = (FeatureSettingAttribute) 
                                 enumItemMember.GetCustomAttributes(typeof(FeatureSettingAttribute), false)
                                           .FirstOrDefault();
                    if(attribute != null)
                    {
                        enumItemFullName = attribute.FullName;
                    }
                }

                return featureManifest[feature].Settings[enumItemFullName ?? enumItemName];
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
        internal static string Setting<TFeatureEnum, TTenantEnum>(this TFeatureEnum feature, Enum settingName,
                                       IFeatureSettingRepository<TFeatureEnum, TTenantEnum> featureSettingRepository)
            where TFeatureEnum : struct
            where TTenantEnum : struct
        {
            try
            {
                var featureSettings = featureSettingRepository.GetFeatureSettings();
                var featureSetting = featureSettings.Where(s => s.Feature.Equals(feature)).First(); //was ==
                Ensure.That <FeatureConfigurationException<TFeatureEnum>>(featureSetting.IsRequiredByFeatureSubsystem,
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