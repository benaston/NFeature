namespace NFeature
{
    using System;

    public class FeatureSettingAvailabilityChecker<TFeatureEnum, TTenantEnum> :
        FeatureSettingAvailabilityChecker<TFeatureEnum, EmptyArgs, TTenantEnum>
        where TFeatureEnum : struct
        where TTenantEnum : struct
    {
        public FeatureSettingAvailabilityChecker(
            Func<FeatureSetting<TFeatureEnum, TTenantEnum>, EmptyArgs, bool> availabilityCheckFunction)
            : base(availabilityCheckFunction) {}
    }
}