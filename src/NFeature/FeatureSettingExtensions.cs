namespace NFeature
{
    using System;
    using System.Linq;

    public static class FeatureSettingExtensions
    {
        public static bool IsEnabled<TFeatureEnumeration>(this FeatureSetting<TFeatureEnumeration> f)
            where TFeatureEnumeration : struct
        {
            return f.FeatureState == FeatureState.Enabled;
        }

        public static bool IsAvailable<TFeatureEnumeration>(this FeatureSetting<TFeatureEnumeration> f, 
                                                            FeatureVisibilityMode m, 
                                                            Tenant tenant,
                                                            DateTime currentDtg)
            where TFeatureEnumeration : struct
        {
            return (f.SupportedTenants.Contains(Tenant.All) || f.SupportedTenants.Contains(tenant)) &&
                   (f.FeatureState == FeatureState.Enabled ||
                    (f.FeatureState == FeatureState.Previewable && m == FeatureVisibilityMode.Preview)) &&
                   f.StartDtg <= currentDtg &&
                   f.EndDtg > currentDtg;
        }

        public static bool IsPreviewable<TFeatureEnumeration>(this FeatureSetting<TFeatureEnumeration> f)
            where TFeatureEnumeration : struct
        {
            return f.FeatureState == FeatureState.Previewable;
        }
    }
}