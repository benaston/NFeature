namespace NFeature
{
    using System.Collections.Generic;

    public interface IFeatureSettingAvailabilityChecker<TFeatureEnumeration, in TAvailabilityCheckArgs>
        where TFeatureEnumeration : struct
    {
        bool RecursivelyCheckAvailability(FeatureSetting<TFeatureEnumeration> featureSettingToCheck,
                                          FeatureSetting<TFeatureEnumeration>[] allFeatureSettings,
                                          TAvailabilityCheckArgs availabilityCheckTuple =
                                              default(TAvailabilityCheckArgs),
                                          List<FeatureSetting<TFeatureEnumeration>> featuresCurrentlyUnderAnalysis =
                                              null);
    }
}