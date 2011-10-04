namespace NFeature
{
    using System;

    public class DatabaseFeatureSettingRepository<TFeatureEnumeration> : IFeatureSettingRepository<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        public FeatureSetting<TFeatureEnumeration>[] GetFeatureSettings()
        {
            throw new NotImplementedException();
        }
    }
}