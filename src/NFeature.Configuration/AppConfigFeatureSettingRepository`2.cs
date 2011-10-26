namespace NFeature.Configuration
{
    using System;
    using System.Linq;

    /// <summary>
    ///   Responsible for retrieving FeatureSettings from a web.config/app.config file.
    /// </summary>
    public class AppConfigFeatureSettingRepository<TFeatureEnum, TTenantEnum> : IFeatureSettingRepository<TFeatureEnum, TTenantEnum>
        where TFeatureEnum :struct
        where TTenantEnum : struct
    {
        public FeatureSetting<TFeatureEnum, TTenantEnum>[] GetFeatureSettings()
        {
            var configElements =
                ConfigurationManager <FeatureConfigurationSection<TFeatureEnum, TTenantEnum>>.Section().FeatureSettings.Cast
                    <FeatureConfigurationElement<TFeatureEnum, TTenantEnum>>();

            return
                configElements.Select(
                    fcse =>
                    new FeatureSetting<TFeatureEnum, TTenantEnum>
                        {
                            IsRequiredByFeatureSubsystem = fcse.IsRequiredByFeatureSubsystem,
                            //this needs to be set first because it affects validation
                            Dependencies = fcse.Dependencies,
                            Feature = (TFeatureEnum)Enum.Parse(typeof(TFeatureEnum), fcse.Name),
                            FeatureState = fcse.State,
                            SupportedTenants = fcse.SupportedTenants,
                            Settings = fcse.Settings,
                            StartDtg = fcse.StartDtg,
                            EndDtg = fcse.EndDtg,
                        }).ToArray();
        }
    }
}