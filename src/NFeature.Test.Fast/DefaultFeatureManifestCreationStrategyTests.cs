 // ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System;
    using System.Web;
    using DefaultImplementations;
    using Exceptions;
    using Moq;
    using NBasicExtensionMethod;
    using NUnit.Framework;
    using TArgs = System.Tuple<FeatureVisibilityMode, Tenant, System.DateTime>;

    [TestFixture]
    [Category("Fast")]
    public class DefaultFeatureManifestCreationStrategyTests
    {
        [Test]
        public void IsAvailable_DisabledAndCookieAvailableAndDependencySettingsOK_ReturnsFalse()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureA,
                                                                FeatureState = FeatureState.Disabled,
                                                                Dependencies = new[] {Feature.TestFeatureE}
                                                            },
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureE,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultManifestCreationStrategy<Feature, Tenant>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var fsSvc = new FeatureSettingService<Feature, Tenant, TArgs>
                            (new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
                                 (DefaultFunctions.AvailabilityCheckFunction), 
                                  fsRepo.Object);

            var m =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object, 
                                                               new ApplicationClock())).GetManifest();

            Assert.That(!Feature.TestFeatureA.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsNotOK_ReturnsFalse()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureA,
                                                                FeatureState = FeatureState.Enabled,
                                                                Dependencies = new[] {Feature.TestFeatureE}
                                                            },
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureE,
                                                                FeatureState = FeatureState.Disabled,
                                                            }
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultManifestCreationStrategy<Feature, Tenant>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var fsSvc = new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
                                 (DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

            var m =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object, 
                                                               new ApplicationClock())).GetManifest();

            Assert.That(!Feature.TestFeatureA.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsNotOK_ReturnsFalse2()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureA,
                                                                FeatureState = FeatureState.Enabled,
                                                                Dependencies = new[] {Feature.TestFeatureE}
                                                            },
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureE,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection());

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var fsSvc = new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
                                 (DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

            var m =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object, 
                                                               new ApplicationClock())).GetManifest();

            Assert.That(!Feature.TestFeatureA.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOKAndEndBeforeStartDate_ThrowsException()
        {
            Assert.Throws<Exception>(
                () =>
                new FeatureSetting<Feature, Tenant>
                    {
                        Feature = Feature.TestFeatureA,
                        FeatureState = FeatureState.Previewable,
                        StartDtg = 1.Day().Ago(),
                        EndDtg = 2.Days().Ago(),
                    });
        }

        [Test]
        public void
            IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOKAndStartAfterEndDate_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () =>
                new FeatureSetting<Feature, Tenant>
                    {
                        Feature = Feature.TestFeatureA,
                        FeatureState = FeatureState.Previewable,
                        StartDtg = 1.Day().Hence(),
                        EndDtg = 2.Days().Hence(),
                    });
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOKAndStartAfterEndDate_ThrowsException()
        {
            Assert.Throws<Exception>(
                () =>
                new FeatureSetting<Feature, Tenant>
                    {
                        Feature = Feature.TestFeatureA,
                        FeatureState = FeatureState.Previewable,
                        StartDtg = 2.Days().Hence(),
                        EndDtg = 1.Days().Hence(),
                    });
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOK_ReturnsTrue()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureA,
                                                                FeatureState = FeatureState.Enabled,
                                                                Dependencies = new[] {Feature.TestFeatureE}
                                                            },
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureE,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultManifestCreationStrategy<Feature, Tenant>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var fsSvc = new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
                                 (DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

            var m =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object, 
                                                               new ApplicationClock())).GetManifest();

            Assert.That(Feature.TestFeatureA.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_NoCorrespondingFeatureSettingAndCookieAvailableAndDependenciesOk_ThrowsException()
        {
            var fsSvc = new Mock<IFeatureSettingService<Feature, Tenant, TArgs>>();
            fsSvc.Setup(
                s =>
                s.AllDependenciesAreSatisfiedForTheFeatureSetting(It.IsAny<FeatureSetting<Feature, Tenant>>(),
                                                                  It.IsAny<TArgs>())).Returns(true);
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new FeatureSetting<Feature, Tenant>[] { }); //important bit  of the setup

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultManifestCreationStrategy<Feature, Tenant>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var m =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc.Object,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object, 
                                                               new ApplicationClock())).GetManifest();

            Assert.Throws<FeatureNotConfiguredException<Feature>>(() => Feature.TestFeatureA.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependenciesOk_ReturnsTrue()
        {
            var fsSvc = new Mock<IFeatureSettingService<Feature, Tenant, TArgs>>();
            fsSvc.Setup(
                s =>
                s.AllDependenciesAreSatisfiedForTheFeatureSetting(It.IsAny<FeatureSetting<Feature, Tenant>>(),
                                                                  It.IsAny<TArgs>())).Returns(true);
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureA,
                                                                FeatureState = FeatureState.Previewable,
                                                            },
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultManifestCreationStrategy<Feature, Tenant>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var m =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc.Object,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object,
                                                               new ApplicationClock())).GetManifest();

            Assert.That(Feature.TestFeatureA.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependencySettingsMissing_ThrowsException()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureA,
                                                                FeatureState = FeatureState.Previewable,
                                                                Dependencies = new[] {Feature.TestFeatureE}
                                                            },
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultManifestCreationStrategy<Feature, Tenant>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var fsSvc = new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
                                 (DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

            var manifestService =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object, 
                                                               new ApplicationClock()));

            Assert.Throws<FeatureNotConfiguredException<Feature>>(() => manifestService.GetManifest());
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependencySettingsOK_ReturnsTrue()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<Feature, Tenant>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureA,
                                                                FeatureState = FeatureState.Previewable,
                                                                Dependencies = new[] {Feature.TestFeatureE}
                                                            },
                                                        new FeatureSetting<Feature, Tenant>
                                                            {
                                                                Feature = Feature.TestFeatureE,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultManifestCreationStrategy<Feature, Tenant>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var tenancyContext = new Mock<ITenancyContext<Tenant>>();
            tenancyContext.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.TenantA);

            var fsSvc = new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
                                 (DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

            var m =
                new FeatureManifestService<Feature>(
                    new DefaultManifestCreationStrategy<Feature, Tenant>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               tenancyContext.Object,
                                                               new ApplicationClock())).GetManifest();

            Assert.That(Feature.TestFeatureA.IsAvailable(m));
        }
    }
}

// ReSharper restore InconsistentNaming