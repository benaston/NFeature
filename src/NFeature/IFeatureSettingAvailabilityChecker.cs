namespace NFeature
{
    using System;
    using System.Collections.Generic;

    public interface IFeatureSettingAvailabilityChecker<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        bool CheckAvailability(FeatureSetting<TFeatureEnumeration> featureSettingToCheck,
                                         FeatureSetting<TFeatureEnumeration>[] allFeatureSettings,
                                         FeatureVisibilityMode inPreviewMode,
                                         Tenant tenant,
                                         DateTime currentDtg,
                                         List<FeatureSetting<TFeatureEnumeration>> featuresCurrentlyUnderAnalysis = null);
    }
}