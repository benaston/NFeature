namespace NFeature
{
    /// <summary>
    ///   Responsible for encapsulating functionality related to 
    ///   FeatureManifests that makes more sense to be placed on a 
    ///   service type.        
    ///   NOTE 1: BA; we equate the domain ID with the tenant ID. 
    ///   Check!
    /// </summary>
    public interface IFeatureManifestService<TFeatureEnum>
        where TFeatureEnum : struct
    {
        IFeatureManifest<TFeatureEnum> GetManifest();
    }
}