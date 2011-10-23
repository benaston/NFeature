namespace NFeature
{
    using System;
    using System.Linq;

    public static class FeatureSettingExtensions
    {
        public static bool IsAvailable<TFeatureEnum, TTenantEnum>(this FeatureSetting<TFeatureEnum, TTenantEnum> f, 
                                                            FeatureVisibilityMode m, 
                                                            TTenantEnum tenant,
                                                            DateTime currentDtg)
            where TFeatureEnum : struct
            where TTenantEnum : struct
        {
            return (f.SupportedTenants.Contains((TTenantEnum)Enum.ToObject(typeof(TTenantEnum), 0)) || f.SupportedTenants.Contains(tenant)) &&
                   (f.FeatureState == FeatureState.Enabled ||
                    (f.FeatureState == FeatureState.Previewable && m == FeatureVisibilityMode.Preview)) &&
                   f.StartDtg <= currentDtg &&
                   f.EndDtg > currentDtg;
        }
    }
}