using System.Web;
using System.Web.Optimization;

namespace UTM.WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/css/font-awesome.min.css",            
                "~/Content/css/style.css",
                "~/Content/css/responsive.css"));  
            
            bundles.Add(new StyleBundle("~/bundles/css2").Include(
                "~/Content/css/font-bootstrap.css"));  
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/js/jquery-3.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/js/custom.js"));
        }
    }
}