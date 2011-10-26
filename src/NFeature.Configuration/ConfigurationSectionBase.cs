namespace NFeature.Configuration
{
    using System.Configuration;

    public abstract class ConfigurationSectionBase : ConfigurationSection
    {
        public abstract string SectionName { get; }

        public virtual ConfigurationSectionBase OnMissingConfiguration()
        {
            return null;
        }
    }
}