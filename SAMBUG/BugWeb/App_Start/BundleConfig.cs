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
                cdnJsAngularIcons =
                    "http://cdnjs.cloudflare.com/ajax/libs/angular-material-icons/0.6.0/angular-material-icons.min.js";

            bundles
                .Add(new ScriptBundle("~/bundles/angular")
                    .Include("~/Scripts/angular.js",
                        "~/Scripts/angular-resource.js",
                        "~/Scripts/angular-route.js",
                        "~/Scripts/angular-messages.js",
                        "~/Scripts/angular-aria.js",
                        "~/Scripts/angular-animate.js",
                        "~/Scripts/angular-material.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/angular-icons", cdnJsAngularIcons)
                    .Include("~/Scripts/angular-material-icons.js", "~/Scripts/svg-morpheus.js"));

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
                .Add(new ScriptBundle("~/bundles/app")
                    .Include("~/App/app.js", 
                    "~/App/Controllers/registerDialogCtrl.js",
                    "~/App/Controllers/loginDialogCtrl.js",
                    "~/App/Controllers/appCtrl.js"
                    ));

            bundles
                .Add(new ScriptBundle("~/bundles/typeahead", cdnJsTypeAhead)
                    .Include("~/Scripts/typeahead.bundle.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/bootstrap-tagsinput")
                    .Include("~/Scripts/bootstrap-tagsinput.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/vendor/xdate")
                    .Include("~/Scripts/xdate.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/common")
                    .Include("~/App/Services/reportingDataService.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/tables")
                    .Include("~/App/Services/tableService.js", "~/App/Controllers/tablesCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/charts")
                    .Include("~/App/Services/chartService.js", "~/App/Controllers/chartsCtrl.js"));
                
            bundles
                .Add(new ScriptBundle("~/bundles/reporting/map")
                    .Include("~/App/Services/mapService.js", "~/App/Controllers/mapCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/farmmanagement/editFarms")
                    .Include("~/App/Controllers/editBlockDialogCtrl.js",
                    "~/App/Controllers/deleteBlockDialogCtrl.js",
                    "~/App/Controllers/deleteFarmDialogCtrl.js",
                        "~/App/Controllers/addBlockDialogCtrl.js",
                        "~/App/Controllers/addFarmDialogCtrl.js",
                        "~/App/Controllers/editFarmsCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/farmmanagement/spraydata")
                    .Include("~/App/Controllers/addTreatmentDialogCtrl.js",
                    "~/App/Controllers/sprayDataCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/chartist", cdnJsChartist)
                    .Include("~/Scripts/chartist.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/chartist-plugins")
                    .Include("~/Scripts/chartist-plugin-vertical-line.js",
                        "~/Scripts/chartist-plugin-tooltip.js",
                        "~/Scripts/chartist-plugin-axistitle.js"));

            bundles
                .Add(new StyleBundle("~/Content/css/angular")
                    .Include("~/Content/angular-material.css"));

            bundles
                .Add(new StyleBundle("~/Content/css/angular-icons")
                    .Include("~/Content/angular-material-icons.css"));

            bundles
                .Add(new StyleBundle("~/Content/css")
                    .Include("~/Content/Site.css"));

            bundles
                .Add(new StyleBundle("~/Content/css/bootstrap")
                    .Include("~/Content/bootstrap.css"));

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
                .Add(new StyleBundle("~/Content/css/typeahead")
                    .Include("~/Content/typeahead.css"));
        }
    }
}
