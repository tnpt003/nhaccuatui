using System.Web;
using System.Web.Optimization;

namespace nhaccuatui
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new Bundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js"));

            //bundles.Add(new StyleBundle("~/Content/css").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/Admin").Include("~/css/Admin/Admin.css"));
            bundles.Add(new StyleBundle("~/Data").Include("~/css/Admin/Data.css"));
            bundles.Add(new StyleBundle("~/Menu").Include("~/css/Content/Menu.css"));
            bundles.Add(new StyleBundle("~/main").Include("~/css/Content/main.css"));
            bundles.Add(new StyleBundle("~/Footer").Include("~/css/Content/Footer.css"));
            bundles.Add(new StyleBundle("~/Core").Include("~/css/Core/Core.css"));
            bundles.Add(new StyleBundle("~/Login-register").Include("~/css/LoginRegister/Login-register.css"));
        }
    }
}
