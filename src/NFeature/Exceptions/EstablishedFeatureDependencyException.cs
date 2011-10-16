namespace NFeature.Exceptions
{
    using NHelpfulException;

    public class EstablishedFeatureDependencyException<TFeature> : HelpfulException
        where TFeature : struct
    {
        private const string DefaultProblemDescription =
            @"Dependencies of established features must be established themselves. Established feature '{0}' depends upon non-established feature '{1}'.";

        private static readonly string[] ResolutionSuggestions = new[]
                                                                     {
                                                                         "Check feature configuration.",
                                                                     };

        public EstablishedFeatureDependencyException(TFeature feature, TFeature dependency)
            : base(string.Format(DefaultProblemDescription, feature, dependency), ResolutionSuggestions, null) {}
    }
}