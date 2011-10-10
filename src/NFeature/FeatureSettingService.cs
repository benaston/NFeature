namespace NFeature
{
    public class FeatureSettingService<TFeatureEnumeration, TAvailabilityCheckArgs> : IFeatureSettingService<TFeatureEnumeration, TAvailabilityCheckArgs>
        where TFeatureEnumeration : struct
    {
        private readonly IFeatureSettingAvailabilityChecker<TFeatureEnumeration, TAvailabilityCheckArgs> _featureSettingDependencyChecker;
        private readonly IFeatureSettingRepository<TFeatureEnumeration> _featureSettingRepository;

        public FeatureSettingService(IFeatureSettingAvailabilityChecker<TFeatureEnumeration, TAvailabilityCheckArgs> featureSettingDependencyChecker,
                                     IFeatureSettingRepository<TFeatureEnumeration> featureSettingRepository)
        {
            _featureSettingDependencyChecker = featureSettingDependencyChecker;
            _featureSettingRepository = featureSettingRepository;
        }

        public bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnumeration> f, 
                                                                    TAvailabilityCheckArgs availabilityCheckArgs)
        {
            return _featureSettingDependencyChecker.RecursivelyCheckAvailability(f,
                                                                      _featureSettingRepository.GetFeatureSettings(),
                                                                      availabilityCheckArgs);
        }
    }
}
