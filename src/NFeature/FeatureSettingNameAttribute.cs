namespace NFeature
{
    using System;

    [AttributeUsage(AttributeTargets.Field)]
    public class FeatureSettingAttribute : Attribute
    {
        public string FullName { get; set; }
    }
}