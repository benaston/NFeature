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

namespace NFeature.DefaultImplementations
{
	using System;
	using System.Globalization;
	using System.Web;

	/// <summary>
	/// 	Can be used to instantiate an instance of 
	/// 	ApplicationClockWithOffset. Checks the 
	/// 	querystring and if it matches the 
	/// 	requirement for performing an offset, 
	/// 	applies it to the clock.
	/// </summary>
	public static class ApplicationClockWithOffsetFactory
	{
		public static ApplicationClockWithOffset CreateFromQueryString(string queryStringFieldName,
		                                                               string expectedQueryStringFormat,
		                                                               string dtgCultureIdentifier)
		{
			var offset = TimeSpan.Zero;
			var queryStringOffset = HttpContext.Current.Request.QueryString[queryStringFieldName];
			if (queryStringOffset != null)
			{
				offset = TimeSpan.ParseExact(queryStringOffset, expectedQueryStringFormat,
				                             new CultureInfo(dtgCultureIdentifier), TimeSpanStyles.None);
			}

			return new ApplicationClockWithOffset(offset);
		}
	}
}