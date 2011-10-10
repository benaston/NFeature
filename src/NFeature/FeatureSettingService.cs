namespace NFeature
{
    public class FeatureSettingService<TFeatureEnumeration> : IFeatureSettingService<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        private readonly IFeatureSettingAvailabilityChecker<TFeatureEnumeration> _featureSettingDependencyChecker;
        private readonly IFeatureSettingRepository<TFeatureEnumeration> _featureSettingRepository;
        private readonly IApplicationClock _systemDtg;

        public FeatureSettingService(IFeatureSettingAvailabilityChecker<TFeatureEnumeration> featureSettingDependencyChecker,
                                     IFeatureSettingRepository<TFeatureEnumeration> featureSettingRepository,
                                     IApplicationClock systemDtg = null)
        {
            _featureSettingDependencyChecker = featureSettingDependencyChecker;
            _featureSettingRepository = featureSettingRepository;
            _systemDtg = systemDtg ?? new ApplicationClock();
        }

        public bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnumeration> f,
                                                                    FeatureVisibilityMode featureConfigurationMode,
                                                                    ITenancyContext tenancyContext)
        {
            return _featureSettingDependencyChecker.CheckAvailability(f,
                                                                                _featureSettingRepository.
                                                                                    GetFeatureSettings(),
                                                                                featureConfigurationMode,
                                                                                tenancyContext.CurrentTenant,
                                                                                _systemDtg.Now);
        }
    }
}
