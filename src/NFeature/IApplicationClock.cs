namespace NFeature
{
    using System;

    public interface IApplicationClock { DateTime Now {get;}}

    public class DefaultApplicationClock : IApplicationClock {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}