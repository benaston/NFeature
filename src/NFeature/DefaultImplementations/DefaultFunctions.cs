namespace NFeature.DefaultImplementations
{
    using System;
    using System.Linq;

    public static class DefaultFunctions
    {
        /// <summary>
        ///   The default imeplementation of the availability checker.
        ///   Checks the tenancy, feature state and date/time.
        ///   Can of course be substituted for by your own function.
        ///   For example, your own function might take into consideration 
        ///   a number indicating site load or user role.
        /// </summary>
        /// <example>
        /// <![CDATA[
        /// return 
        /// (s.FeatureState == FeatureState.Enabled ||
        /// (s.FeatureState == FeatureState.Previewable && args.Item1 == FeatureVisibilityMode.Preview)) &&
        /// s.StartDtg <= args.Item3 &&
        /// s.EndDtg > args.Item3;
        /// ]]>
        /// </example>
        public static bool AvailabilityCheckFunction<TFeatureEnum, TTenantEnum>(
            FeatureSetting<TFeatureEnum, TTenantEnum> s,
            Tuple<FeatureVisibilityMode, TTenantEnum, DateTime> args)
            where TFeatureEnum : struct
            where TTenantEnum : struct
        {
            return (s.SupportedTenants.Contains((TTenantEnum)Enum.ToObject(typeof(TTenantEnum), 0)) || s.SupportedTenants.Contains(args.Item2)) &&
                   (s.FeatureState == FeatureState.Enabled ||
                    (s.FeatureState == FeatureState.Previewable && args.Item1 == FeatureVisibilityMode.Preview)) &&
                   s.StartDtg <= args.Item3 &&
                   s.EndDtg > args.Item3;
        }
    }
}