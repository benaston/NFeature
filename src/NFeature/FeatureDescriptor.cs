// Copyright 2012, Ben Aston (ben@bj.ma).
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
	using System.Collections.Generic;
	using Exceptions;

	/// <summary>
	/// 	See comments on iface.
	/// </summary>
	public class FeatureDescriptor<TFeatureEnum> : IFeatureDescriptor<TFeatureEnum>
		where TFeatureEnum : struct
	{
		private bool _isAvailable;

		public FeatureDescriptor(TFeatureEnum feature) {
			Feature = feature;
		}

		public TFeatureEnum Feature { get; set; }

		public bool IsEstablished { get; set; }

		public bool IsAvailable {
			get {
				if (IsEstablished) {
					throw new EstablishedFeatureAvailabilityCheckException<TFeatureEnum>(Feature);
				}

				return _isAvailable;
			}
			set { _isAvailable = value; }
		}

		public IList<TFeatureEnum> Dependencies { get; set; }

		public IDictionary<string, dynamic> Settings { get; set; }
	}
}