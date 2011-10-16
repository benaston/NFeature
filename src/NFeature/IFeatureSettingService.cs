namespace NFeature
{
    /// <summary>
    ///   Responsible for encapsulating functionality related to 
    ///   FeatureSetting that makes more sense to be placed on a 
    ///   service type.
    /// </summary>
    public interface IFeatureSettingService<TFeatureEnum, TTenantEnum, in TAvailabilityCheckArgs>
        where TFeatureEnum : struct
        where TTenantEnum : struct
    {
        /// <summary>
        ///   Determines whether the dependencies are 
        ///   satisfied for the specified feature setting.
        /// </summary>
        bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnum, TTenantEnum> f,
                                                             TAvailabilityCheckArgs availabilityCheckArgs);
    }
}