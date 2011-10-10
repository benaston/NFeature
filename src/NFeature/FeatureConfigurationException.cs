namespace NFeature
{
    using System;
    using NHelpfulException;

    public class FeatureConfigurationException<TFeatureEnumeration> : HelpfulException
        where TFeatureEnumeration : struct
    {
        public FeatureConfigurationException(string message)
            : base(string.Format("{0}", message)) {}

        public FeatureConfigurationException(TFeatureEnumeration feature, string message, Exception innerException)
            : base(
                string.Format("{0}. Affected feature: \"{1}\".", message,
                              Enum.GetName(typeof (TFeatureEnumeration), feature)), innerException: innerException) {}
    }
}