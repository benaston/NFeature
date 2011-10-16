namespace NFeature.Exceptions
{
    using NHelpfulException;

    public class EstablishedFeatureAvailabilityCheckException<TFeature> : HelpfulException
        where TFeature : struct
    {
        private const string DefaultProblemDescription =
            @"Feature '{0}' is established and may not be queried for availability.";

        private static readonly string[] ResolutionSuggestions = new[]
                                                                     {
                                                                         "Check feature configuration.",
                                                                         "Remove availability check."
                                                                     };

        public EstablishedFeatureAvailabilityCheckException(TFeature f)
            : base(string.Format(DefaultProblemDescription, f), ResolutionSuggestions, null) {}
    }
}