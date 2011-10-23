namespace NFeature
{
    using DefaultImplementations;

    public class AppConfigFeatureSettingRepository2<TFeatureEnum> : AppConfigFeatureSettingRepository<TFeatureEnum, DefaultTenantEnum>
        where TFeatureEnum : struct {}
}