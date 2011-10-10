namespace NFeature
{
    using System;
    using NHelpfulException;

    public class FeatureNotAvailableException : HelpfulException
    {
        public FeatureNotAvailableException(string problemDescription,
                                            string[] resolutionSuggestions = default(string[]),
                                            Exception innerException = default(Exception))
            : base(problemDescription, resolutionSuggestions, innerException) {}
    }
}