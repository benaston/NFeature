namespace NFeature.Test.Slow
{
    public static class FeatureSettingNames
    {
        public enum TestFeatureE
        {
            [FeatureSetting(FullName = "My.Type, MyAssembly")]
            AssemblyName,
            SimpleSetting,
        }
    }
}