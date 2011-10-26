namespace NFeature.Configuration
{
    public class AppConfigFeatureSettingRepository<TFeatureEnum> : 
        AppConfigFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum>,
        IFeatureSettingRepository<TFeatureEnum>
        where TFeatureEnum : struct {}
}