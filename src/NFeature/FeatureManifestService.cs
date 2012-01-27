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
		/// 	Uses the supplied strategy to retrieve the FeatureManifest.
		/// </summary>
		public IFeatureManifest<TFeatureEnum> GetManifest()
		{
			return _manifestCreationStrategy.CreateFeatureManifest();
		}
	}
}