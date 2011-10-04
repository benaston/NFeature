namespace NFeature
{
    using System;
    using System.Linq;

    /// <summary>
    ///   Responsible for retrieving FeatureSettings from a web.config file.
    /// </summary>
    public class WebConfigFeatureSettingRepository<TFeatureEnumeration> : IFeatureSettingRepository<TFeatureEnumeration>
        where TFeatureEnumeration :struct
    {
        public FeatureSetting<TFeatureEnumeration>[] GetFeatureSettings()
        {
            var configElements =
                ConfigurationManager < FeatureConfigurationSection<TFeatureEnumeration>>.Section().FeatureSettings.Cast
                    < FeatureConfigurationElement<TFeatureEnumeration>>();

            return
                configElements.Select(
                    fcse =>
                    new FeatureSetting<TFeatureEnumeration>
                        {
                            IsRequiredByFeatureSubsystem = fcse.IsRequiredByFeatureSubsystem,
                            //this needs to be set first because it affects validation
                            Dependencies = fcse.Dependencies,
                            Feature = (TFeatureEnumeration)Enum.Parse(typeof(TFeatureEnumeration), fcse.Name),
                            FeatureState = fcse.State,
                            SupportedTenants = fcse.SupportedTenants,
                            Settings = fcse.Settings,
                            StartDtg = fcse.StartDtg,
                            EndDtg = fcse.EndDtg,
                        }).ToArray();
        }
    }
}