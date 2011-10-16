namespace NFeature.Exceptions
{
    using System;
    using NHelpfulException;

    public class FeatureNotConfiguredException<TFeatureEnum> : HelpfulException
        where TFeatureEnum : struct
    {
        public FeatureNotConfiguredException(TFeatureEnum feature, Exception innerException)
            : base(
                string.Format("Feature configuration not found for \"{0}\".", Enum.GetName(typeof(TFeatureEnum), feature)),
                innerException:innerException) {}
    }
}