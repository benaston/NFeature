namespace NFeature
{
    public class FeatureSettingService<TFeatureEnumeration, TAvailabilityCheckArgs> : IFeatureSettingService<TFeatureEnumeration, TAvailabilityCheckArgs>
        where TFeatureEnumeration : struct
    {
        private readonly IFeatureSettingAvailabilityChecker<TFeatureEnumeration, TAvailabilityCheckArgs> _featureSettingDependencyChecker;
        private readonly IFeatureSettingRepository<TFeatureEnumeration> _featureSettingRepository;
        //private readonly TAvailabilityCheckArgs _availabilityCheckArgs;
        //private readonly IApplicationClock _systemDtg;

        public FeatureSettingService(IFeatureSettingAvailabilityChecker<TFeatureEnumeration, TAvailabilityCheckArgs> featureSettingDependencyChecker,
                                     IFeatureSettingRepository<TFeatureEnumeration> featureSettingRepository)
                                     //TAvailabilityCheckArgs availabilityCheckArgs)
        {
            _featureSettingDependencyChecker = featureSettingDependencyChecker;
            _featureSettingRepository = featureSettingRepository;
            //_availabilityCheckArgs = availabilityCheckArgs;
            //_systemDtg = systemDtg ?? new ApplicationClock();
        }

        public bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnumeration> f, TAvailabilityCheckArgs availabilityCheckArgs)
                                                                    //FeatureVisibilityMode featureConfigurationMode,
                                                                    //ITenancyContext tenancyContext)
        {
            return _featureSettingDependencyChecker.RecursivelyCheckAvailability(f,
                                                                      _featureSettingRepository.GetFeatureSettings(),
                                                                      availabilityCheckArgs);
        }
    }
}
