namespace NFeature
{
    using System.Collections.Generic;

    /// <summary>
    ///   See comments on iface.
    /// </summary>
    public class FeatureDescriptor<TFeatureEnumeration> : IFeatureDescriptor<TFeatureEnumeration>
        where TFeatureEnumeration : struct
    {
        public FeatureDescriptor(TFeatureEnumeration feature)
        {
            Feature = feature;
        }

        public TFeatureEnumeration Feature { get; set; }

        public bool IsEstablished{ get; set; }

        private bool _isAvailable;
        public bool IsAvailable
        {
            get
            {
                if (IsEstablished)
                {
                    throw new AvailabilityCheckOnEstablishedFeatureException<TFeatureEnumeration>(Feature);
                }

                return _isAvailable; 
            }
            set { _isAvailable = value; }
        }

        public IList<TFeatureEnumeration> Dependencies { get; set; }

        public IDictionary<string, string> Settings { get; set; }
    }
}