namespace NFeature
{
    using System.Collections.Generic;
    using Configuration;
    using DefaultImplementations;

    public interface IFeatureSettingAvailabilityChecker<TFeatureEnum> : IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, EmptyArgs>
        where TFeatureEnum : struct
    { }

    public interface IFeatureSettingAvailabilityChecker<TFeatureEnum, in TAvailabilityCheckArgs> : IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs>
        where TFeatureEnum : struct
    {}

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