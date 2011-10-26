namespace NFeature.Configuration
{
    using System;
    using System.Configuration;

    public static class ConfigurationManager<T> where T : ConfigurationSectionBase, new()
    {
        public static T Section(Func<T> onMissingSection = null)
        {
            var config = new T();
            var section = Section(config.SectionName);

            if (section == null)
            {
                return new T().OnMissingConfiguration() as T;
            }

            return section;
        }

        public static T Section(string sectionName)
        {
            return (T) ConfigurationManager.GetSection(sectionName);
        }
    }
}