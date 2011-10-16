namespace NFeature
{
    using System.Collections.Generic;

    /// <summary>
    ///   Responsible for encapsulating a record of 
    ///   all the features in the application 
    ///   together with their availability.
    /// </summary>
    /// <typeparam name="TFeatureEnum">The enumeration type used to define the features in the system.</typeparam>
    public interface IFeatureManifest<TFeatureEnum> :
        IDictionary<TFeatureEnum, IFeatureDescriptor<TFeatureEnum>>
        where TFeatureEnum : struct {}
}