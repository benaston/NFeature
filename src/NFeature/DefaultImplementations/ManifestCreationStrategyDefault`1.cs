namespace NFeature.DefaultImplementations
{
    using Configuration;

    public class ManifestCreationStrategyDefault<TFeatureEnum> :
        ManifestCreationStrategyDefault<TFeatureEnum, DefaultTenantEnum>
        where TFeatureEnum : struct
    {
        public ManifestCreationStrategyDefault(
            IFeatureSettingRepository<TFeatureEnum> featureSettingRepository,
            IFeatureSettingService<TFeatureEnum> featureSettingService)
            : base(featureSettingRepository, featureSettingService) {}
    }
}