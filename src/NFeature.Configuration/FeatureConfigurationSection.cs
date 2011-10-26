namespace NFeature.Configuration
{
    using System.ComponentModel;
    using System.Configuration;

    /// <summary>
    ///   Based upon de-compilation of the connection strings configuration section.
    /// </summary>
    public class FeatureConfigurationSection<TFeatureEnum, TTenantEnum> : ConfigurationSectionBase
        where TFeatureEnum  :struct
        where TTenantEnum : struct
    {
        private static readonly ConfigurationProperty ConfigurationProperties =
            new ConfigurationProperty(null,
                                      typeof (FeatureConfigurationElementCollection<TFeatureEnum, TTenantEnum>),
                                      null,
                                      ConfigurationPropertyOptions.IsDefaultCollection);

        [TypeConverter(typeof (CommaDelimitedStringCollectionConverter))]
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public FeatureConfigurationElementCollection<TFeatureEnum, TTenantEnum> FeatureSettings
        {
            get { return ((FeatureConfigurationElementCollection<TFeatureEnum, TTenantEnum>) base[ConfigurationProperties]); }
        }

        public override string SectionName
        {
            get { return "features"; }
        }
    }
}