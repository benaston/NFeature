using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFeature
{
    using DefaultImplementations;

    public class FeatureSettingAvailabilityChecker<TFeatureEnum> :
        FeatureSettingAvailabilityChecker<TFeatureEnum, EmptyArgs, DefaultTenantEnum>
        where TFeatureEnum : struct
    {
        public FeatureSettingAvailabilityChecker(Func<FeatureSetting<TFeatureEnum, DefaultTenantEnum>,
            EmptyArgs, bool> availabilityCheckFunction)
            : base(availabilityCheckFunction) { }
    }
}
