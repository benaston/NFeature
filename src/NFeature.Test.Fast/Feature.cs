namespace NFeature.Test.Fast
{
    /// <summary>
    ///   There should be an entry in this enumeration for each feature 
    ///   that can be configured off or on using the feature configuration 
    ///   subsystem.
    ///   NOTE: BA; NEVER EVER use the actual integer value associated 
    ///   with the feature in your code, because it will probably change.
    ///   NOTE: BA; please try and keep in alphabetical order for clarity.
    /// </summary>
    public enum TestFeatureList
    {
        TestFeature1 = 1,
        TestFeature2,
        TestFeature3,
        TestFeature4,
        TestFeature5,
        TestFeature6,
    }
}