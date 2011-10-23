namespace NFeature.DefaultImplementations
{
    public class ManifestCreationStrategyDefault<TFeatureEnum, TTenantEnum> :
        IFeatureManifestCreationStrategy<TFeatureEnum>
        where TFeatureEnum : struct
        where TTenantEnum : struct
    {
        private readonly IFeatureSettingRepository<TFeatureEnum, TTenantEnum> _featureSettingRepository;
        private readonly IFeatureSettingService<TFeatureEnum, TTenantEnum, EmptyArgs> _featureSettingService;

        public ManifestCreationStrategyDefault(IFeatureSettingRepository<TFeatureEnum, TTenantEnum> featureSettingRepository,
                                               IFeatureSettingService<TFeatureEnum, TTenantEnum, EmptyArgs> featureSettingService)
        {
            _featureSettingRepository = featureSettingRepository;
            _featureSettingService = featureSettingService;
        }

        public IFeatureManifest<TFeatureEnum> CreateFeatureManifest()
        {
            var featureSettings = _featureSettingRepository.GetFeatureSettings();
            var manifest = new FeatureManifest<TFeatureEnum>();

            foreach (var setting in featureSettings)
            {
                var isAvailable = _featureSettingService
                    .AllDependenciesAreSatisfiedForTheFeatureSetting(setting, new EmptyArgs());

                manifest.Add(setting.Feature,
                             new FeatureDescriptor<TFeatureEnum>(setting.Feature)
                                 {
                                     Dependencies = setting.Dependencies,
                                     IsAvailable = isAvailable,
                                     Settings = setting.Settings,
                                 });
            }

            return manifest;
        }
    }
}