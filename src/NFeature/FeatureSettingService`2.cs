namespace NFeature
{
    using Configuration;
    using DefaultImplementations;

    public class FeatureSettingService<TFeatureEnum, TAvailabilityCheckArgs> : IFeatureSettingService<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs>
        where TFeatureEnum : struct
    {
        private readonly IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs> _featureSettingAvailabilityChecker;
        private readonly IFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum> _featureSettingRepository;

        public FeatureSettingService(IFeatureSettingAvailabilityChecker<TFeatureEnum, DefaultTenantEnum, TAvailabilityCheckArgs> featureSettingAvailabilityChecker,
                                     IFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum> featureSettingRepository)
        {
            _featureSettingAvailabilityChecker = featureSettingAvailabilityChecker;
            _featureSettingRepository = featureSettingRepository;
        }

        public bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnum, DefaultTenantEnum> f, 
                                                                    TAvailabilityCheckArgs availabilityCheckArgs)
        {
            return _featureSettingAvailabilityChecker.RecursivelyCheckAvailability(f,
                                                                      _featureSettingRepository.GetFeatureSettings(),
                                                                      availabilityCheckArgs);
        }
    }
}
