namespace NFeature
{
    using NHelpfulException;

    public class AvailabilityCheckOnEstablishedFeatureException<TFeature> : HelpfulException
        where TFeature : struct
    {
        private const string DefaultProblemDescription =
            @"Feature '{0}' is established and may not be queried for availability.";

        private static readonly string[] ResolutionSuggestions = new[]
                                                                     {
                                                                         "Check feature configuration.",
                                                                         "Remove availability check."
                                                                     };

        public AvailabilityCheckOnEstablishedFeatureException(TFeature f)
            : base(string.Format(DefaultProblemDescription, ResolutionSuggestions), null) {}
    }
}