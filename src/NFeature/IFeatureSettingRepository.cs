namespace NFeature
{
    /// <summary>
    ///   Responsible for providing an abstraction 
    ///   for the retrieval of feature settings from a store.
    /// </summary>
    public interface IFeatureSettingRepository<TFeatureEnum, TTenantEnum>
        where TFeatureEnum : struct
        where TTenantEnum : struct
    {
        FeatureSetting<TFeatureEnum, TTenantEnum>[] GetFeatureSettings();
    }
}