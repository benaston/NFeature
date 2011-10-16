namespace NFeature.Exceptions
{
    using System;

    /// <summary>
    ///   Indicates that a feature depends ultimately 
    ///   upon itself being available, which is an 
    ///   invalid dependency.
    /// </summary>
    public class CircularFeatureSettingDependencyException : Exception {}
}