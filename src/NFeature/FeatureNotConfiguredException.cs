namespace NFeature
{
    using System;
    using NHelpfulException;

    public class FeatureNotConfiguredException<TFeatureEnumeration> : HelpfulException
        where TFeatureEnumeration : struct
    {
        public FeatureNotConfiguredException(TFeatureEnumeration feature, Exception innerException)
            : base(
                string.Format("Feature configuration not found for \"{0}\".", Enum.GetName(typeof(TFeatureEnumeration), feature)),
                innerException:innerException) {}
    }
}