namespace NFeature
{
    /// <summary>
    ///   Responsible for encapsulating functionality related to 
    ///   FeatureManifests that makes more sense to be placed on a 
    ///   service type.
    /// </summary>
    public interface IFeatureManifestService<TFeatureEnum>
        where TFeatureEnum : struct
    {
        IFeatureManifest<TFeatureEnum> GetManifest();
    }
}