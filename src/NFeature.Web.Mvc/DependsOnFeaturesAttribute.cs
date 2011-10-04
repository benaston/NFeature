namespace NFeature.Web.Mvc
{
    using System.Web.Mvc;
    using NSure;

    public class DependsOnFeaturesAttribute<TFeatureEnumeration> : ActionFilterAttribute
        where TFeatureEnumeration : struct
    {
        public TFeatureEnumeration[] Features { get; set; }

        public DependsOnFeaturesAttribute(TFeatureEnumeration[] features)
        {
            Features = features;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var featureManifest = (IFeatureManifest<TFeatureEnumeration>)ServiceLocator.Instance.Get(typeof(IFeatureManifest));
            Ensure.That(featureManifest.IsNotNull(), "featureManifest not available.");

            if (!Feature.DocumentDownloadsController.IsAvailable(featureManifest))
            {
                filterContext.Result = new HttpNotFoundResult();
                
                return;
            }

            base.OnActionExecuting(filterContext);           
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
                filterContext.HttpContext.Trace.Write("(Logging Filter)Exception thrown");

            base.OnActionExecuted(filterContext);
        }
    }
}