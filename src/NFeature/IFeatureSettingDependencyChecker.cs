namespace NFeature
{
    using System;
    using System.Collections.Generic;

    public interface IFeatureSettingDependencyChecker<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        bool AreDependenciesMetForTenant(FeatureSetting<TFeatureEnumeration> featureSettingToCheck,
                                         FeatureSetting<TFeatureEnumeration>[] allFeatureSettings,
                                         FeatureVisibilityMode inPreviewMode,
                                         Tenant tenant,
                                         DateTime currentDtg,
                                         List<FeatureSetting<TFeatureEnumeration>> featuresCurrentlyUnderAnalysis = null);
    }
}