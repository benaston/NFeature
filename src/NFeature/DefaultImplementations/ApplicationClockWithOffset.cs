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

namespace NFeature.DefaultImplementations
{
	using System;

	/// <summary>
	/// 	An implementation of a clock with an offset. Can be used to enable testing of features that will only be available at some time in the future (because it enables simulation of travelling into the future).
	/// </summary>
	public class ApplicationClockWithOffset
	{
		public ApplicationClockWithOffset()
		{
			SystemOffset = TimeSpan.Zero;
		}

		public ApplicationClockWithOffset(TimeSpan systemOffset)
		{
			SystemOffset = systemOffset;
		}

		public TimeSpan SystemOffset { get; private set; }

		public DateTime Now
		{
			get { return DateTime.Now.Add(SystemOffset); }
		}
	}
}