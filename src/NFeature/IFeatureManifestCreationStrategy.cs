namespace NFeature
{
    /// <summary>
    ///   Responsible for defining the interface for types that
    ///   provide functionality for feature manifest creation.
    ///   These strategies might for example, take into account 
    ///   cookie configuration, domain configuration and user role.
    /// </summary>
    public interface IFeatureManifestCreationStrategy<TFeatureEnumeration>
        where TFeatureEnumeration : struct 
    {
        IFeatureManifest<TFeatureEnumeration> CreateFeatureManifest();
    }
}