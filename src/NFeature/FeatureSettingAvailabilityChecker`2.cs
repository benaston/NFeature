namespace NFeature
{
    using System;
    using Configuration;
    using DefaultImplementations;

    public class FeatureSettingAvailabilityChecker<TFeatureEnum, TAvailabilityCheckArgs> :
        FeatureSettingAvailabilityChecker<TFeatureEnum, TAvailabilityCheckArgs, DefaultTenantEnum>
        where TFeatureEnum : struct
    {
        public FeatureSettingAvailabilityChecker(
            Func<FeatureSetting<TFeatureEnum, DefaultTenantEnum>, TAvailabilityCheckArgs, bool> availabilityCheckFunction)
            : base(availabilityCheckFunction) { }
    }
}