namespace NFeature.DefaultImplementations
{
    using System;

    /// <summary>
    ///   Default implementation of the application clock. 
    ///   Your own implementation might implement an offset 
    ///   feature, depending on user-set locale.
    /// </summary>
    public class ApplicationClock : IApplicationClock
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}