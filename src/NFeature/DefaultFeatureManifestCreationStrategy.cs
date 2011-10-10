namespace NFeature
{
    using System.Web;
    using NBasicExtensionMethod;
    using TArgs = System.Tuple<FeatureVisibilityMode, Tenant, System.DateTime>;

    /// <summary>
    ///   Constructs the feature manifest according to 
    ///   the tenancy context, the existence of the 
    ///   preview cookie and feature configuration.
    ///   This provides an example manifest creation
    ///   strategy, and is replaceable (being a strategy).
    /// </summary>
    public class CookieBasedPreviewManifestCreationStrategy<TFeatureEnumeration> :
        IFeatureManifestCreationStrategy<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        public const string FeaturePreviewCookieName = "FeaturePreviewCookie";

        private readonly IFeatureSettingService<TFeatureEnumeration, TArgs> _featureSettingService;
        private readonly IFeatureSettingRepository<TFeatureEnumeration> _featureSettingsRepository;
        private readonly HttpContextBase _httpContext;
        private readonly ITenancyContext _tenancyContext;
        private readonly IApplicationClock _clock;

        public CookieBasedPreviewManifestCreationStrategy(
            IFeatureSettingService<TFeatureEnumeration, TArgs> featureSettingService,
            IFeatureSettingRepository<TFeatureEnumeration> featureSettingsRepository,
            HttpContextBase httpContext,
            ITenancyContext tenancyContext,
            IApplicationClock clock)
        {
            _featureSettingService = featureSettingService;
            _featureSettingsRepository = featureSettingsRepository;
            _httpContext = httpContext;
            _tenancyContext = tenancyContext;
            _clock = clock;
        }

        public IFeatureManifest<TFeatureEnumeration> CreateFeatureManifest()
        {
            var featureSettings = _featureSettingsRepository.GetFeatureSettings();
            var manifest = new FeatureManifest<TFeatureEnumeration>();

            foreach (var setting in featureSettings)
            {
                var featureVisibilityMode = _httpContext.Request.Cookies[FeaturePreviewCookieName].IsNotNull()
                                                   ? FeatureVisibilityMode.Preview
                                                   : FeatureVisibilityMode.Normal;

                var isAvailable = _featureSettingService
                    .AllDependenciesAreSatisfiedForTheFeatureSetting(setting, new TArgs(featureVisibilityMode, _tenancyContext.CurrentTenant, _clock.Now));
                                                                                                         

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