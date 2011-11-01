namespace NFeature
{
    using Configuration;

    public class FeatureSettingService<TFeatureEnum> : IFeatureSettingService<TFeatureEnum>
        where TFeatureEnum : struct

    {
        private readonly IFeatureSettingAvailabilityChecker<TFeatureEnum> _featureSettingAvailabilityChecker;
        
        private readonly IFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum> _featureSettingRepository;

        public FeatureSettingService(IFeatureSettingAvailabilityChecker<TFeatureEnum> featureSettingAvailabilityChecker,
                                     IFeatureSettingRepository<TFeatureEnum> featureSettingRepository)
        {
            _featureSettingAvailabilityChecker = featureSettingAvailabilityChecker;
            _featureSettingRepository = featureSettingRepository;
        }

        public bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnum, DefaultTenantEnum> f, 
                                                                    EmptyArgs availabilityCheckArgs)
        {
            return _featureSettingAvailabilityChecker.RecursivelyCheckAvailability(f,
                                                                      _featureSettingRepository.GetFeatureSettings(),
                                                                      availabilityCheckArgs);
        }
    }
}
