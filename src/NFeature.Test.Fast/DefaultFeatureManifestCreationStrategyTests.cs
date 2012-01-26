// Copyright 2011, Ben Aston (ben@bj.ma.)
// 
// This file is part of NFeature.
// 
// NFeature is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// NFeature is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with NFeature.  If not, see <http://www.gnu.org/licenses/>.

namespace NFeature.Test.Fast
{
	using System.Web;
	using Configuration;
	using DefaultImplementations;
	using Exceptions;
	using Moq;
	using NBasicExtensionMethod;
	using NHelpfulException;
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
				                   	{
				                   		new HttpCookie(
				                   			ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>.
				                   				FeaturePreviewCookieName)
				                   	});

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
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc,
					                                                                                 fsRepo.Object,
					                                                                                 httpContext.Object,
					                                                                                 tenancyContext.Object,
					                                                                                 new ApplicationClock())).
					GetManifest();

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
				                   	{
				                   		new HttpCookie(
				                   			ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>.
				                   				FeaturePreviewCookieName)
				                   	});

			var httpContext = new Mock<HttpContextBase>();
			httpContext.Setup(
				s =>
				s.Request).Returns(request.Object);

			var tenancyContext = new Mock<ITenancyContext<Tenant>>();
			tenancyContext.Setup(
				s =>
				s.CurrentTenant).Returns(Tenant.TenantA);

			var fsSvc =
				new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
				                                                  	(DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

			var m =
				new FeatureManifestService<Feature>(
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc,
					                                                                                 fsRepo.Object,
					                                                                                 httpContext.Object,
					                                                                                 tenancyContext.Object,
					                                                                                 new ApplicationClock())).
					GetManifest();

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

			var fsSvc =
				new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
				                                                  	(DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

			var m =
				new FeatureManifestService<Feature>(
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc,
					                                                                                 fsRepo.Object,
					                                                                                 httpContext.Object,
					                                                                                 tenancyContext.Object,
					                                                                                 new ApplicationClock())).
					GetManifest();

			Assert.That(!Feature.TestFeatureA.IsAvailable(m));
		}

		[Test]
		public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOKAndEndBeforeStartDate_ThrowsException()
		{
			Assert.Throws<HelpfulException>(
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
			Assert.Throws<HelpfulException>(
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
				                   	{
				                   		new HttpCookie(
				                   			ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>.
				                   				FeaturePreviewCookieName)
				                   	});

			var httpContext = new Mock<HttpContextBase>();
			httpContext.Setup(
				s =>
				s.Request).Returns(request.Object);

			var tenancyContext = new Mock<ITenancyContext<Tenant>>();
			tenancyContext.Setup(
				s =>
				s.CurrentTenant).Returns(Tenant.TenantA);

			var fsSvc =
				new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
				                                                  	(DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

			var m =
				new FeatureManifestService<Feature>(
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc,
					                                                                                 fsRepo.Object,
					                                                                                 httpContext.Object,
					                                                                                 tenancyContext.Object,
					                                                                                 new ApplicationClock())).
					GetManifest();

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
				s.GetFeatureSettings()).Returns(new FeatureSetting<Feature, Tenant>[] {}); //important bit  of the setup

			var request = new Mock<HttpRequestBase>();
			request.Setup(
				s =>
				s.Cookies).Returns(new HttpCookieCollection
				                   	{
				                   		new HttpCookie(
				                   			ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>.
				                   				FeaturePreviewCookieName)
				                   	});

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
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc.Object,
					                                                                                 fsRepo.Object,
					                                                                                 httpContext.Object,
					                                                                                 tenancyContext.Object,
					                                                                                 new ApplicationClock())).
					GetManifest();

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
				                   	{
				                   		new HttpCookie(
				                   			ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>.
				                   				FeaturePreviewCookieName)
				                   	});

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
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc.Object,
					                                                                                 fsRepo.Object,
					                                                                                 httpContext.Object,
					                                                                                 tenancyContext.Object,
					                                                                                 new ApplicationClock())).
					GetManifest();

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
				                   	{
				                   		new HttpCookie(
				                   			ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>.
				                   				FeaturePreviewCookieName)
				                   	});

			var httpContext = new Mock<HttpContextBase>();
			httpContext.Setup(
				s =>
				s.Request).Returns(request.Object);

			var tenancyContext = new Mock<ITenancyContext<Tenant>>();
			tenancyContext.Setup(
				s =>
				s.CurrentTenant).Returns(Tenant.TenantA);

			var fsSvc =
				new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
				                                                  	(DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

			var manifestService =
				new FeatureManifestService<Feature>(
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc,
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
				                   	{
				                   		new HttpCookie(
				                   			ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>.
				                   				FeaturePreviewCookieName)
				                   	});

			var httpContext = new Mock<HttpContextBase>();
			httpContext.Setup(
				s =>
				s.Request).Returns(request.Object);

			var tenancyContext = new Mock<ITenancyContext<Tenant>>();
			tenancyContext.Setup(
				s =>
				s.CurrentTenant).Returns(Tenant.TenantA);

			var fsSvc =
				new FeatureSettingService<Feature, Tenant, TArgs>(new FeatureSettingAvailabilityChecker<Feature, TArgs, Tenant>
				                                                  	(DefaultFunctions.AvailabilityCheckFunction), fsRepo.Object);

			var m =
				new FeatureManifestService<Feature>(
					new ManifestCreationStrategyConsideringStateCookieTenantAndTime<Feature, Tenant>(fsSvc,
					                                                                                 fsRepo.Object,
					                                                                                 httpContext.Object,
					                                                                                 tenancyContext.Object,
					                                                                                 new ApplicationClock())).
					GetManifest();

			Assert.That(Feature.TestFeatureA.IsAvailable(m));
		}
	}
}

// ReSharper restore InconsistentNaming