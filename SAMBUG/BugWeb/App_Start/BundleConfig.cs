using System;
using System.Web;
using System.Web.Optimization;

namespace BugWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;

            const string cdnJsChartist = "https://cdnjs.cloudflare.com/ajax/libs/chartist/0.9.4/chartist.min.js",
                cdnCssChartist = "https://cdnjs.cloudflare.com/ajax/libs/chartist/0.9.4/chartist.min.css",
                cdnJsTypeAhead = "https://cdnjs.cloudflare.com/ajax/libs/typeahead.js/0.11.1/typeahead.bundle.js",
                cdnCssSpinner = "http://css-spinners.com/css/spinner/whirly.css";

            bundles
                .Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/Scripts/jquery-{version}.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles
                .Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*"));

            bundles
                .Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/customJS")
                .Include("~/Scripts/MapControls.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/typeahead", cdnJsTypeAhead)
                .Include("~/Scripts/typeahead.bundle.js"));

            bundles
               .Add(new ScriptBundle("~/bundles/bootstrap-tagsinput")
               .Include("~/Scripts/bootstrap-tagsinput.js"));
           
            //TODO: this script bundle must include all scripts needed by tables and charts, rename as needed
            bundles
                .Add(new ScriptBundle("~/bundles/reporting")
                .Include("~/Scripts/charts.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/chartist", cdnJsChartist)
                .Include("~/Scripts/chartist.js"));
                
            bundles
                .Add(new StyleBundle("~/Content/css")
                .Include("~/Content/bootstrap.css", "~/Content/site.css"));

            bundles
               .Add(new StyleBundle("~/Content/css/bootstrap-tagsinput")
               .Include("~/Content/bootstrap-tagsinput.css"));

            //TODO: this style bundle must include all styles needed by tables and charts, rename as needed
            bundles
                .Add(new StyleBundle("~/Content/css/reporting")
                .Include("~/Content/chartist.min.css", "~/Content/charts.css"));

            bundles
                .Add(new StyleBundle("~/Content/css/chartist", cdnCssChartist)
                .Include("~/Content/chartist.min.css"));

            bundles
                .Add(new StyleBundle("~/Content/css/spinner", cdnCssSpinner));

        }
    }
}
