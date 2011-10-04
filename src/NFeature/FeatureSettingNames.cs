namespace NFeature
{
    public static class FeatureSettingNames
    {        
        public enum CharitySelfSignUpEmail
        {
            JustGivingEmailRecipientEmailAddress,
            JustGivingEmailRecipientFirstName,
            EmailHeaderImageUri,
            CharityApplicationPackUri,
            CharityApplicationPackLinkText,
            HmrcFormUri,
            HmrcFormLinkText,
        }

        public enum DocumentDownloadsController
        {
            RouteBase, //e.g. justgiving.com/RouteBase/myfolder/mydoc.pdf
            ActualFilesystemDirectory, //eg. d:\website\ActualFilesystemDirectory\mydoc.pdf
        }

        public enum ExampleFeature
        {
            ExampleSettingName,
        }

        public enum ApplcationClock
        {
            OffsetTimeSpanCultureIdentifier,
            OffsetTimeSpanFormat,
            OffsetQueryStringFieldName,
        }
    }
}