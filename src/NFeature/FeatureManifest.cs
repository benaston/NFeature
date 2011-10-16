namespace NFeature
{
    using System.Collections.Generic;

    /// <summary>
    ///   See notes on iface.
    /// </summary>
    public class FeatureManifest<TFeatureEnum> :
        Dictionary<TFeatureEnum, IFeatureDescriptor<TFeatureEnum>>, IFeatureManifest<TFeatureEnum>
        where TFeatureEnum : struct {}
}