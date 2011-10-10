namespace NFeature
{
    /// <summary>
    ///   Responsible for encapsulating functionality related to 
    ///   FeatureSetting that makes more sense to be placed on a 
    ///   service type.
    /// </summary>
    public interface IFeatureSettingService<TFeatureEnumeration, in TAvailabilityCheckArgs>
        where TFeatureEnumeration : struct
    {
        /// <summary>
        ///   Determines whether the dependencies are 
        ///   satisfied for the specified feature setting.
        /// </summary>
        bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnumeration> f,
                                                             TAvailabilityCheckArgs availabilityCheckArgs);
    }
}