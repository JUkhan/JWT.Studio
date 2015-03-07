using System.Web;
using System.Web.Optimization;

namespace Components
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                       "~/Content/toastr/toastr.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome/css/font-awesome.css",
                     "~/Content/toastr/toastr.css",
                     "~/Scripts/UI-Grid/ui-grid-unstable.css"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/uigrid").Include(
                     "~/Scripts/UI-Grid/csv.js",
                      "~/Scripts/UI-Grid/pdfmake.js",
                       "~/Scripts/UI-Grid/vfs_fonts.js",
                        "~/Scripts/UI-Grid/ui-grid-unstable.js"
                     ));
        }
    }
}
