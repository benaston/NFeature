namespace NFeature
{
    public class FeatureManifestService<TFeatureEnumeration> : IFeatureManifestService<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        private readonly IFeatureManifestCreationStrategy<TFeatureEnumeration> _manifestCreationStrategy;

        public FeatureManifestService(IFeatureManifestCreationStrategy<TFeatureEnumeration> manifestCreationStrategy)
        {
            _manifestCreationStrategy = manifestCreationStrategy;
        }

        /// <summary>
        ///   Uses the supplied strategy to retrieve the FeatureManifest.
        /// </summary>
        public IFeatureManifest<TFeatureEnumeration> GetManifest()
        {
            return _manifestCreationStrategy.CreateFeatureManifest();
        }
    }
}