namespace NFeature.Exceptions
{
    using System;
    using NHelpfulException;

    public class FeatureDependencyConfigurationException : HelpfulException
    {
        public FeatureDependencyConfigurationException(string message, Exception innerException)
            : base(message, innerException:innerException) {}
    }
}