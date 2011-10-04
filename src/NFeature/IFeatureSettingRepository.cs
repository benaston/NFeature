namespace NFeature
{
    /// <summary>
    ///   Responsible for providing an abstraction 
    ///   for the retrieval of feature settings from a store.
    /// </summary>
    public interface IFeatureSettingRepository<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        FeatureSetting<TFeatureEnumeration>[] GetFeatureSettings();
    }
}