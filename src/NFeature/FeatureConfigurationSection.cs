namespace NFeature
{
    using System.ComponentModel;
    using System.Configuration;

    /// <summary>
    ///   Based upon de-compilation of the connection strings configuration section.
    /// </summary>
    public class FeatureConfigurationSection<TFeatureEnumeration> : ConfigurationSectionBase
        where TFeatureEnumeration  :struct
    {
        private static readonly ConfigurationProperty ConfigurationProperties =
            new ConfigurationProperty(null,
                                      typeof (FeatureConfigurationElementCollection<TFeatureEnumeration>),
                                      null,
                                      ConfigurationPropertyOptions.IsDefaultCollection);

        [TypeConverter(typeof (CommaDelimitedStringCollectionConverter))]
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public FeatureConfigurationElementCollection<TFeatureEnumeration> FeatureSettings
        {
            get { return ((FeatureConfigurationElementCollection<TFeatureEnumeration>) base[ConfigurationProperties]); }
        }

        public override string SectionName
        {
            get { return "features"; }
        }
    }
}