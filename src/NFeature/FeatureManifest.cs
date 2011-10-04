namespace NFeature
{
    using System.Collections.Generic;

    /// <summary>
    ///   See notes on iface.
    /// </summary>
    public class FeatureManifest<TFeatureEnumeration> :
        Dictionary<TFeatureEnumeration, IFeatureDescriptor<TFeatureEnumeration>>, IFeatureManifest<TFeatureEnumeration>
        where TFeatureEnumeration : struct {}
}