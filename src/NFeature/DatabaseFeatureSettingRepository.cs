namespace NFeature
{
    using System;

    /// <summary>
    ///   Only the AppConfig feature setting repo implementation 
    ///   exists currently.
    /// </summary>
    /// <typeparam name = "TFeatureEnumeration"></typeparam>
    public class DatabaseFeatureSettingRepository<TFeatureEnumeration> : IFeatureSettingRepository<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        public FeatureSetting<TFeatureEnumeration>[] GetFeatureSettings()
        {
            throw new NotImplementedException();
        }
    }
}