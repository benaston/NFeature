namespace NFeature
{
    public class FeatureSettingService<TFeatureEnum, TTenantEnum, TAvailabilityCheckArgs> : IFeatureSettingService<TFeatureEnum, TTenantEnum, TAvailabilityCheckArgs>
        where TFeatureEnum : struct
        where TTenantEnum : struct
    {
        private readonly IFeatureSettingAvailabilityChecker<TFeatureEnum, TTenantEnum, TAvailabilityCheckArgs> _featureSettingAvailabilityChecker;
        private readonly IFeatureSettingRepository<TFeatureEnum, TTenantEnum> _featureSettingRepository;

        public FeatureSettingService(IFeatureSettingAvailabilityChecker<TFeatureEnum, TTenantEnum, TAvailabilityCheckArgs> featureSettingAvailabilityChecker,
                                     IFeatureSettingRepository<TFeatureEnum, TTenantEnum> featureSettingRepository)
        {
            _featureSettingAvailabilityChecker = featureSettingAvailabilityChecker;
            _featureSettingRepository = featureSettingRepository;
        }

        public bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnum, TTenantEnum> f, 
                                                                    TAvailabilityCheckArgs availabilityCheckArgs)
        {
            return _featureSettingAvailabilityChecker.RecursivelyCheckAvailability(f,
                                                                      _featureSettingRepository.GetFeatureSettings(),
                                                                      availabilityCheckArgs);
        }
    }
}
