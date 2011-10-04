namespace NFeature
{
    using System;

    public interface IApplicationClock { DateTime Now {get;}}

    public class ApplicationClock : IApplicationClock {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}