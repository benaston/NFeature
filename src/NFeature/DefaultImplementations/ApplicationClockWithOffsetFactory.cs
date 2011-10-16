namespace NFeature.DefaultImplementations
{
    using System;
    using System.Globalization;
    using System.Web;

    /// <summary>
    /// Can be used to instantiate an instance of ApplicationClockWithOffset.
    /// Checks the querystring and if it matches the requirement for performing 
    /// an offset, applies it to the clock.
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