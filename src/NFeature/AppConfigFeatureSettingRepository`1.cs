namespace NFeature
{
    using DefaultImplementations;

    public class AppConfigFeatureSettingRepository<TFeatureEnum> : 
        AppConfigFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum>,
        IFeatureSettingRepository<TFeatureEnum>
        where TFeatureEnum : struct {}
}