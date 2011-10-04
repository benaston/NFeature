namespace NFeature
{
    public class FeatureSettingService<TFeatureEnumeration> : IFeatureSettingService<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        private readonly IFeatureSettingDependencyChecker<TFeatureEnumeration> _featureSettingDependencyChecker;
        private readonly IFeatureSettingRepository<TFeatureEnumeration> _featureSettingRepository;
        private readonly IApplicationClock _systemDtg;

        public FeatureSettingService(IFeatureSettingDependencyChecker<TFeatureEnumeration> featureSettingDependencyChecker,
                                     IFeatureSettingRepository<TFeatureEnumeration> featureSettingRepository,
                                     IApplicationClock systemDtg)
        {
            _featureSettingDependencyChecker = featureSettingDependencyChecker;
            _featureSettingRepository = featureSettingRepository;
            _systemDtg = systemDtg;
        }

        public bool AllDependenciesAreSatisfiedForTheFeatureSetting(FeatureSetting<TFeatureEnumeration> f,
                                                                    FeatureVisibilityMode featureConfigurationMode,
                                                                    ITenancyContext tenancyContext)
        {
            return _featureSettingDependencyChecker.AreDependenciesMetForTenant(f,
                                                                                _featureSettingRepository.
                                                                                    GetFeatureSettings(),
                                                                                featureConfigurationMode,
                                                                                tenancyContext.CurrentTenant,
                                                                                _systemDtg.Now);
        }
    }
}
