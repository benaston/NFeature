namespace NFeature.Exceptions
{
    using System;
    using NHelpfulException;

    public class FeatureConfigurationException<TFeatureEnum> : HelpfulException
        where TFeatureEnum : struct
    {
        public FeatureConfigurationException(string message)
            : base(string.Format("{0}", message)) {}

        public FeatureConfigurationException(TFeatureEnum feature, string message, Exception innerException)
            : base(
                string.Format("{0}. Affected feature: \"{1}\".", message,
                              Enum.GetName(typeof (TFeatureEnum), feature)), innerException: innerException) {}
    }
}