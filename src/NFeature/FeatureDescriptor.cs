namespace NFeature
{
    using System.Collections.Generic;
    using Exceptions;

    /// <summary>
    ///   See comments on iface.
    /// </summary>
    public class FeatureDescriptor<TFeatureEnum> : IFeatureDescriptor<TFeatureEnum>
        where TFeatureEnum : struct
    {
        public FeatureDescriptor(TFeatureEnum feature)
        {
            Feature = feature;
        }

        public TFeatureEnum Feature { get; set; }

        public bool IsEstablished{ get; set; }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get
            {
                if (IsEstablished)
                {
                    throw new EstablishedFeatureAvailabilityCheckException<TFeatureEnum>(Feature);
                }

                return _isAvailable; 
            }
            set { _isAvailable = value; }
        }

        public IList<TFeatureEnum> Dependencies { get; set; }

        public IDictionary<string, string> Settings { get; set; }
    }
}