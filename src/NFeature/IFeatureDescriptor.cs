namespace NFeature
{
    using System.Collections.Generic;

    /// <summary>
    ///   A description of the state of a feature.
    ///   Basically metadata for members of the 
    ///   Feature enumeration.
    /// </summary>
    public interface IFeatureDescriptor<TFeatureEnumeration>
    {
        /// <summary>
        ///   Calculated by the manifest creation strategy.
        ///   Might be based on feature dependencies and 
        ///   cookies, for example.
        /// </summary>
        bool IsAvailable { get; set; }

        IList<TFeatureEnumeration> Dependencies { get; set; }

        IDictionary<string, string> Settings { get; set; }
    }
}