namespace NFeature.DefaultImplementations
{
    using System;

    /// <summary>
    ///   An implementation of a clock with an offset.
    ///   Can be used to enable testing of features
    ///   that will only be available at some time in 
    ///   the future (because it enables simulation 
    ///   of travelling into the future).
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