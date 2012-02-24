// Copyright 2011, Ben Aston (ben@bj.ma).
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
	using System;
	using Configuration;
	using Exceptions;
	using NBasicExtensionMethod;
	using NUnit.Framework;

	/// <summary>
	/// Please accept my apologies for the indentation of this file.
	/// I am slowly correcting an earlier ReSharper-ism.
	/// </summary>
	[TestFixture]
	[Category("Fast")]
	public class FeatureSettingAvailabilityCheckerTests
	{
		private readonly
			FeatureSettingAvailabilityChecker
				<Feature, Tuple<FeatureVisibilityMode, Tenant, DateTime>, Tenant>
			_dependencyChecker = new FeatureSettingAvailabilityChecker<Feature,
				Tuple<FeatureVisibilityMode, Tenant, DateTime>, Tenant>
				((s, args) => s.IsAvailable(args.Item1, args.Item2, args.Item3));

		[Test]
		public void RecursivelyCheckAvailability_ADependsOnBAndAAndBStartDatesAreInPast_AIsAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
					StartDtg =
						1.Day().Ago(),
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
					StartDtg =
						1.Day().Ago(),
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                            allFeatureSettings,
			                                                            new Tuple
			                                                            	<FeatureVisibilityMode, Tenant,
			                                                            	DateTime>(
			                                                            	FeatureVisibilityMode.Normal,
			                                                            	Tenant.All,
			                                                            	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndAStartDateIsInFuture_AIsNotAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
					StartDtg =
						1.Day().Hence(),
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                             allFeatureSettings,
			                                                             new Tuple
			                                                             	<FeatureVisibilityMode, Tenant
			                                                             	, DateTime>(
			                                                             	FeatureVisibilityMode.Normal,
			                                                             	Tenant.All,
			                                                             	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndBDependsOnA_CircularDependencyExceptionIsThrown() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new[]
						{Feature.TestFeatureA},
					FeatureState =
						FeatureState.Enabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.Throws<CircularFeatureSettingDependencyException>(
				() => _dependencyChecker.RecursivelyCheckAvailability(featureSetting,
				                                                      allFeatureSettings,
				                                                      new Tuple
				                                                      	<FeatureVisibilityMode, Tenant,
				                                                      	DateTime>(
				                                                      	FeatureVisibilityMode.Normal,
				                                                      	Tenant.All,
				                                                      	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndBDependsOnCAndAIsNotEnabled_AIsAvailable_BecauseAIsDisabled
			() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Disabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new[]
						{Feature.TestFeatureC},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureC,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                             allFeatureSettings,
			                                                             new Tuple
			                                                             	<FeatureVisibilityMode, Tenant
			                                                             	, DateTime>(
			                                                             	FeatureVisibilityMode.Normal,
			                                                             	Tenant.All,
			                                                             	DateTime.Now)));
		}

		[Test]
		public void RecursivelyCheckAvailability_ADependsOnBAndBDependsOnCAndAllEnabled_AIsAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new[]
						{Feature.TestFeatureC},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureC,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                            allFeatureSettings,
			                                                            new Tuple
			                                                            	<FeatureVisibilityMode, Tenant,
			                                                            	DateTime>(
			                                                            	FeatureVisibilityMode.Normal,
			                                                            	Tenant.All,
			                                                            	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndBDependsOnCAndBAndCAreNotEnabled_AIsNotAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new[]
						{Feature.TestFeatureC},
					FeatureState =
						FeatureState.Disabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureC,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Disabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                             allFeatureSettings,
			                                                             new Tuple
			                                                             	<FeatureVisibilityMode, Tenant
			                                                             	, DateTime>(
			                                                             	FeatureVisibilityMode.Normal,
			                                                             	Tenant.All,
			                                                             	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndBDependsOnCAndBIsNotEnabled_AIsNotAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new[]
						{Feature.TestFeatureC},
					FeatureState =
						FeatureState.Disabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureC,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                             allFeatureSettings,
			                                                             new Tuple
			                                                             	<FeatureVisibilityMode, Tenant
			                                                             	, DateTime>(
			                                                             	FeatureVisibilityMode.Normal,
			                                                             	Tenant.All,
			                                                             	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndBDependsOnCAndCIsNotEnabled_AIsNotAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature = Feature.TestFeatureA,
					Dependencies = new[] {Feature.TestFeatureB},
					FeatureState = FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new[]
						{Feature.TestFeatureC},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureC,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Disabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                             allFeatureSettings,
			                                                             new Tuple
			                                                             	<FeatureVisibilityMode, Tenant
			                                                             	, DateTime>(
			                                                             	FeatureVisibilityMode.Normal,
			                                                             	Tenant.All,
			                                                             	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndBIsPreviewableAndModeIsNormal_AIsNotAvailable_BecauseModeShouldBePreview
			() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.
							Previewable,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                             allFeatureSettings,
			                                                             new Tuple
			                                                             	<FeatureVisibilityMode, Tenant
			                                                             	, DateTime>(
			                                                             	FeatureVisibilityMode.Normal,
			                                                             	Tenant.All,
			                                                             	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndBStartDateIsInFuture_AIsNotAvailable_BecauseBIsNotYetAvailable
			() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
					StartDtg =
						1.Day().Hence(),
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(!_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                             allFeatureSettings,
			                                                             new Tuple
			                                                             	<FeatureVisibilityMode, Tenant
			                                                             	, DateTime>(
			                                                             	FeatureVisibilityMode.Normal,
			                                                             	Tenant.All,
			                                                             	DateTime.Now)));
		}

		[Test]
		public void RecursivelyCheckAvailability_ADependsOnBAndBStartDateIsInPast_AIsAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
					StartDtg =
						1.Day().Ago(),
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                            allFeatureSettings,
			                                                            new Tuple
			                                                            	<FeatureVisibilityMode, Tenant,
			                                                            	DateTime>(
			                                                            	FeatureVisibilityMode.Normal,
			                                                            	Tenant.All,
			                                                            	DateTime.Now)));
		}

		[Test]
		public void RecursivelyCheckAvailability_ADependsOnBAndBothEnabled_AIsAvailable() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[]
						{Feature.TestFeatureB},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                            allFeatureSettings,
			                                                            new Tuple
			                                                            	<FeatureVisibilityMode, Tenant,
			                                                            	DateTime>(
			                                                            	FeatureVisibilityMode.Normal,
			                                                            	Tenant.All,
			                                                            	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_ADependsOnBAndDAndBDependsOnCAndAllAreEnabled_AIsEnabled()
			//a single feature can have multiple dependencies
		{
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureA,
					Dependencies =
						new[] {
							Feature.
								TestFeatureB,
							Feature.
								TestFeatureD
						},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureB,
					Dependencies =
						new[]
						{Feature.TestFeatureC},
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureC,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
				},
				new FeatureSetting<Feature, Tenant> {
					Feature =
						Feature.TestFeatureD,
					Dependencies =
						new Feature[0],
					FeatureState =
						FeatureState.Enabled,
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.That(_dependencyChecker.RecursivelyCheckAvailability(featureSetting,
			                                                            allFeatureSettings,
			                                                            new Tuple
			                                                            	<FeatureVisibilityMode, Tenant,
			                                                            	DateTime>(
			                                                            	FeatureVisibilityMode.Normal,
			                                                            	Tenant.All,
			                                                            	DateTime.Now)));
		}

		[Test]
		public void
			RecursivelyCheckAvailability_AIsEstablishedAndDependsOnBAndBIsNotEstablished_ExceptionIsThrown_BecauseAllDependenciesOfEstablishedFeaturesMustThemselvesBeEstablished
			() {
			var allFeatureSettings = new[] {
				new FeatureSetting<Feature, Tenant> //A
				{
					Feature = Feature.TestFeatureA,
					Dependencies =
						new[] {Feature.TestFeatureB},
					FeatureState = FeatureState.Established,
					StartDtg = 1.Day().Ago(),
				},
				new FeatureSetting<Feature, Tenant> //B
				{
					Feature = Feature.TestFeatureB,
					Dependencies = new Feature[0],
					FeatureState = FeatureState.Enabled,
					StartDtg = 1.Day().Ago(),
				},
			};
			FeatureSetting<Feature, Tenant> featureSetting = allFeatureSettings[0];

			Assert.Throws<EstablishedFeatureDependencyException<Feature>>(
				() => _dependencyChecker.RecursivelyCheckAvailability(featureSetting,
				                                                      allFeatureSettings,
				                                                      new Tuple
				                                                      	<FeatureVisibilityMode, Tenant,
				                                                      	DateTime>(
				                                                      	FeatureVisibilityMode.Normal,
				                                                      	Tenant.All,
				                                                      	DateTime.Now)
				      	));
		}
	}
}