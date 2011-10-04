namespace NFeature
{
    using System.Collections.Generic;

    /// <summary>
    ///   See comments on iface.
    /// </summary>
    public class FeatureDescriptor<TFeatureEnumeration> : IFeatureDescriptor<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        public bool IsAvailable { get; set; }

        public IList<TFeatureEnumeration> Dependencies { get; set; }

        public IDictionary<string, string> Settings { get; set; }
    }
}