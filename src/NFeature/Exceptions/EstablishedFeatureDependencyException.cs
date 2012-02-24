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

namespace NFeature.Exceptions
{
	using NHelpfulException;

	public class EstablishedFeatureDependencyException<TFeature> : HelpfulException
		where TFeature : struct
	{
		private const string DefaultProblemDescription =
			@"Dependencies of established features must be established themselves. Established feature '{0}' depends upon non-established feature '{1}'.";

		private static readonly string[] ResolutionSuggestions = new[] {
			"Check feature configuration."
			,
		};

		public EstablishedFeatureDependencyException(TFeature feature, TFeature dependency)
			: base(
				string.Format(DefaultProblemDescription, feature, dependency), ResolutionSuggestions, null) {}
	}
}