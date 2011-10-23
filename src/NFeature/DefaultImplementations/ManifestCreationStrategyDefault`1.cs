namespace NFeature.DefaultImplementations
{
    public class ManifestCreationStrategyDefault<TFeatureEnum> :
        ManifestCreationStrategyDefault<TFeatureEnum, DefaultTenantEnum>
        where TFeatureEnum : struct
    {
        public ManifestCreationStrategyDefault(
            IFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum> featureSettingRepository,
            IFeatureSettingService<TFeatureEnum, DefaultTenantEnum, EmptyArgs> featureSettingService)
            : base(featureSettingRepository, featureSettingService) {}
    }
}