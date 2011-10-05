namespace NFeature
{
    using System.Web;
    using NBasicExtensionMethod;

    /// <summary>
    ///   Constructs the feature manifest according to 
    ///   the tenancy context, the existence of the 
    ///   preview cookie and feature configuration.
    ///   This provides an example manifest creation
    ///   strategy, and is replaceable (being a strategy).
    /// </summary>
    public class DefaultFeatureManifestCreationStrategy<TFeatureEnumeration> :
        IFeatureManifestCreationStrategy<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        public const string FeaturePreviewCookieName = "FeaturePreviewCookie";

        private readonly IFeatureSettingService<TFeatureEnumeration> _featureSettingService;
        private readonly IFeatureSettingRepository<TFeatureEnumeration> _featureSettingsRepository;
        private readonly HttpContextBase _httpContext;
        private readonly ITenancyContext _tenancyContext;

        public DefaultFeatureManifestCreationStrategy(
            IFeatureSettingService<TFeatureEnumeration> featureSettingService,
            IFeatureSettingRepository<TFeatureEnumeration> featureSettingsRepository,
            HttpContextBase httpContext,
            ITenancyContext tenancyContext)
        {
            _featureSettingService = featureSettingService;
            _featureSettingsRepository = featureSettingsRepository;
            _httpContext = httpContext;
            _tenancyContext = tenancyContext;
        }

        public IFeatureManifest<TFeatureEnumeration> CreateFeatureManifest()
        {
            var featureSettings = _featureSettingsRepository.GetFeatureSettings();
            var manifest = new FeatureManifest<TFeatureEnumeration>();

            foreach (var setting in featureSettings)
            {
                var featureConfigurationMode = _httpContext.Request.Cookies[FeaturePreviewCookieName].IsNotNull()
                                                   ? FeatureVisibilityMode.Preview
                                                   : FeatureVisibilityMode.Normal;

                var isAvailable = _featureSettingService.AllDependenciesAreSatisfiedForTheFeatureSetting(setting,
                                                                                                         featureConfigurationMode,
                                                                                                         _tenancyContext);

                manifest.Add(setting.Feature,
                             new FeatureDescriptor<TFeatureEnumeration>(setting.Feature)
                                 {
                                     Dependencies = setting.Dependencies,
                                     IsAvailable = isAvailable,
                                     Settings = setting.Settings,
                                 });
            }

            return manifest;
        }
    }
}