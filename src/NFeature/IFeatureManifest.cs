namespace NFeature
{
    using System.Collections.Generic;

    /// <summary>
    ///   Responsible for encapsulating a record of 
    ///   all the features in the application 
    ///   together with their availability.
    /// </summary>
    public interface IFeatureManifest<TFeatureEnumeration> :
        IDictionary<TFeatureEnumeration, IFeatureDescriptor<TFeatureEnumeration>>
        where TFeatureEnumeration : struct {}
}