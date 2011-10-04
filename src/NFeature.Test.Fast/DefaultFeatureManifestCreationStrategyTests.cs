 // ReSharper disable InconsistentNaming
namespace NFeature.Test.Fast
{
    using System;
    using System.Web;
    using Moq;
    using NBasicExtensionMethod;
    using NFeature;
    using NUnit.Framework;

    [TestFixture]
    [Category("Fast")]
    public class DefaultFeatureManifestCreationStrategyTests
    {
        [Test]
        public void IsAvailable_DisabledAndCookieAvailableAndDependencySettingsOK_ReturnsFalse()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature1,
                                                                FeatureState = FeatureState.Disabled,
                                                                Dependencies = new[] {TestFeatureList.TestFeature5}
                                                            },
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature5,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var fsSvc = new FeatureSettingService<TestFeatureList>(new FeatureSettingDependencyChecker<TestFeatureList>(), fsRepo.Object, new ApplicationClock());

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultFeatureManifestCreationStrategy<TestFeatureList>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var m =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object)).GetManifest();

            Assert.That(!TestFeatureList.TestFeature1.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsNotOK_ReturnsFalse()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature1,
                                                                FeatureState = FeatureState.Enabled,
                                                                Dependencies = new[] {TestFeatureList.TestFeature5}
                                                            },
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature5,
                                                                FeatureState = FeatureState.Disabled,
                                                            }
                                                    });

            var fsSvc = new FeatureSettingService<TestFeatureList>(new FeatureSettingDependencyChecker<TestFeatureList>(), fsRepo.Object, new ApplicationClock());

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultFeatureManifestCreationStrategy<TestFeatureList>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var m =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object)).GetManifest();

            Assert.That(!TestFeatureList.TestFeature1.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsNotOK_ReturnsFalse2()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature1,
                                                                FeatureState = FeatureState.Enabled,
                                                                Dependencies = new[] {TestFeatureList.TestFeature5}
                                                            },
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature5,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var fsSvc = new FeatureSettingService<TestFeatureList>(new FeatureSettingDependencyChecker<TestFeatureList>(), fsRepo.Object, new ApplicationClock());

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection());

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var m =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object)).GetManifest();

            Assert.That(!TestFeatureList.TestFeature1.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOKAndEndBeforeStartDate_ThrowsException()
        {
            Assert.Throws<Exception>(
                () =>
                new FeatureSetting<TestFeatureList>
                    {
                        Feature = TestFeatureList.TestFeature1,
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
                new FeatureSetting<TestFeatureList>
                    {
                        Feature = TestFeatureList.TestFeature1,
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
                new FeatureSetting<TestFeatureList>
                    {
                        Feature = TestFeatureList.TestFeature1,
                        FeatureState = FeatureState.Previewable,
                        StartDtg = 2.Days().Hence(),
                        EndDtg = 1.Days().Hence(),
                    });
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOK_ReturnsTrue()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature1,
                                                                FeatureState = FeatureState.Enabled,
                                                                Dependencies = new[] {TestFeatureList.TestFeature5}
                                                            },
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature5,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var fsSvc = new FeatureSettingService<TestFeatureList>(new FeatureSettingDependencyChecker<TestFeatureList>(), fsRepo.Object, new ApplicationClock());

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultFeatureManifestCreationStrategy<TestFeatureList>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var m =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object)).GetManifest();

            Assert.That(TestFeatureList.TestFeature1.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_NoCorrespondingFeatureSettingAndCookieAvailableAndDependenciesOk_ThrowsException()
        {
            var fsSvc = new Mock<IFeatureSettingService<TestFeatureList>>();
            fsSvc.Setup(
                s =>
                s.AllDependenciesAreSatisfiedForTheFeatureSetting(It.IsAny<FeatureSetting<TestFeatureList>>(),
                                                                  It.IsAny<FeatureVisibilityMode>(),
                                                                  It.IsAny<ITenancyContext>())).Returns(true);
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new FeatureSetting<TestFeatureList>[] { }); //important bit  of the setup

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultFeatureManifestCreationStrategy<TestFeatureList>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var m =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc.Object,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object)).GetManifest();

            Assert.Throws<FeatureNotConfiguredException<TestFeatureList>>(() => TestFeatureList.TestFeature1.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependenciesOk_ReturnsTrue()
        {
            var fsSvc = new Mock<IFeatureSettingService<TestFeatureList>>();
            fsSvc.Setup(
                s =>
                s.AllDependenciesAreSatisfiedForTheFeatureSetting(It.IsAny<FeatureSetting<TestFeatureList>>(),
                                                                  It.IsAny<FeatureVisibilityMode>(),
                                                                  It.IsAny<ITenancyContext>())).Returns(true);
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature1,
                                                                FeatureState = FeatureState.Previewable,
                                                            },
                                                    });

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultFeatureManifestCreationStrategy<TestFeatureList>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var m =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc.Object,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object)).GetManifest();

            Assert.That(TestFeatureList.TestFeature1.IsAvailable(m));
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependencySettingsMissing_ThrowsException()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature1,
                                                                FeatureState = FeatureState.Previewable,
                                                                Dependencies = new[] {TestFeatureList.TestFeature5}
                                                            },
                                                    });

            var fsSvc = new FeatureSettingService<TestFeatureList>(new FeatureSettingDependencyChecker<TestFeatureList>(), fsRepo.Object, new ApplicationClock());

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultFeatureManifestCreationStrategy<TestFeatureList>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var manifestService =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object));

            Assert.Throws<FeatureNotConfiguredException<TestFeatureList>>(() => manifestService.GetManifest());
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependencySettingsOK_ReturnsTrue()
        {
            var fsRepo = new Mock<IFeatureSettingRepository<TestFeatureList>>();
            fsRepo.Setup(
                s =>
                s.GetFeatureSettings()).Returns(new[]
                                                    {
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature1,
                                                                FeatureState = FeatureState.Previewable,
                                                                Dependencies = new[] {TestFeatureList.TestFeature5}
                                                            },
                                                        new FeatureSetting<TestFeatureList>
                                                            {
                                                                Feature = TestFeatureList.TestFeature5,
                                                                FeatureState = FeatureState.Previewable,
                                                            }
                                                    });

            var fsSvc = new FeatureSettingService<TestFeatureList>(new FeatureSettingDependencyChecker<TestFeatureList>(), fsRepo.Object, new ApplicationClock());

            var request = new Mock<HttpRequestBase>();
            request.Setup(
                s =>
                s.Cookies).Returns(new HttpCookieCollection
                                       {new HttpCookie(DefaultFeatureManifestCreationStrategy<TestFeatureList>.FeaturePreviewCookieName)});

            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(
                s =>
                s.Request).Returns(request.Object);

            var domainConfiguration = new Mock<ITenancyContext>();
            domainConfiguration.Setup(
                s =>
                s.CurrentTenant).Returns(Tenant.Tenant1);

            var m =
                new FeatureManifestService<TestFeatureList>(
                    new DefaultFeatureManifestCreationStrategy<TestFeatureList>(fsSvc,
                                                               fsRepo.Object,
                                                               httpContext.Object,
                                                               domainConfiguration.Object)).GetManifest();

            Assert.That(TestFeatureList.TestFeature1.IsAvailable(m));
        }
    }
}

// ReSharper restore InconsistentNaming