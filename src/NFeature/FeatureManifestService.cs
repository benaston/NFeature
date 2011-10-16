namespace NFeature
{
    public class FeatureManifestService<TFeatureEnum> : IFeatureManifestService<TFeatureEnum>
        where TFeatureEnum : struct
    {
        private readonly IFeatureManifestCreationStrategy<TFeatureEnum> _manifestCreationStrategy;

        public FeatureManifestService(IFeatureManifestCreationStrategy<TFeatureEnum> manifestCreationStrategy)
        {
            _manifestCreationStrategy = manifestCreationStrategy;
        }

        /// <summary>
        ///   Uses the supplied strategy to retrieve the FeatureManifest.
        /// </summary>
        public IFeatureManifest<TFeatureEnum> GetManifest()
        {
            return _manifestCreationStrategy.CreateFeatureManifest();
        }
    }
}