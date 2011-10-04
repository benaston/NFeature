namespace NFeature
{
    using System;
    using NHelpfulException;

    public class FeatureNotAvailableException<TFeatureEnumeration> : HelpfulException
        where TFeatureEnumeration : struct
    {
        public FeatureNotAvailableException(TFeatureEnumeration feature)
            : base(
                string.Format("Feature \"{0}\" is not available.", Enum.GetName(typeof (TFeatureEnumeration), feature))) {}

        public FeatureNotAvailableException(TFeatureEnumeration feature, Exception innerException)
            : base(
                string.Format("Feature \"{0}\" is not available.", Enum.GetName(typeof (TFeatureEnumeration), feature)),
                innerException:innerException) { }
    }
}