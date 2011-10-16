namespace NFeature
{
    using System.Collections.Generic;

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