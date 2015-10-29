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
                cdnJsAngularIcons = "http://cdnjs.cloudflare.com/ajax/libs/angular-material-icons/0.6.0/angular-material-icons.min.js",
                cdnJsDataTables = "https://cdn.datatables.net/r/dt/jszip-2.5.0,pdfmake-0.1.18,dt-1.10.9,b-1.0.3,b-html5-1.0.3,b-print-1.0.3,cr-1.2.0,fh-3.0.0,r-1.0.7/datatables.min.js",
                cdnCssDataTables = "https://cdn.datatables.net/r/dt/jszip-2.5.0,pdfmake-0.1.18,dt-1.10.9,b-1.0.3,b-html5-1.0.3,b-print-1.0.3,cr-1.2.0,fh-3.0.0,r-1.0.7/datatables.min.css";

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
                    .Include("~/Scripts/angular-material-icons.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/angular-icons-morpheus")
                    .Include("~/Scripts/svg-morpheus.js"));


            bundles
                .Add(new ScriptBundle("~/bundles/jquery")
                    .Include("~/Scripts/jquery-{version}.js"));

            /*bundles
                .Add(new ScriptBundle("~/bundles/jqueryval")
                    .Include("~/Scripts/jquery.validate*"));*/

            bundles
               .Add(new ScriptBundle("~/bundles/linqjs")
                   .Include("~/Scripts/linq.js"));

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
                .Add(new ScriptBundle("~/bundles/vendor/xdate")
                    .Include("~/Scripts/xdate.js"));
            
            bundles
                .Add(new ScriptBundle("~/bundles/index")
                    .Include("~/App/Controllers/indexCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/common")
                    .Include("~/App/Services/commonReportingService.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/tables")
                    .Include("~/App/Services/tableService.js", "~/App/Controllers/tablesCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/vendor/dataTables", cdnJsDataTables)
                    .Include("~/Scripts/DataTables/jquery.dataTables.js", "~/Scripts/DataTables/.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/charts")
                    .Include("~/App/Services/chartService.js", "~/App/Controllers/chartsCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/vendor/bspline")
                    .Include("~/Scripts/bspline.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/map")
                    .Include("~/App/Services/mapService.js", "~/App/Controllers/mapCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/farmmanagement/editFarms")
                    .Include("~/App/Services/farmManagementService.js",
                    "~/App/Controllers/editBlockDialogCtrl.js",
                    "~/App/Controllers/deleteBlockDialogCtrl.js",
                    "~/App/Controllers/deleteFarmDialogCtrl.js",
                        "~/App/Controllers/addBlockDialogCtrl.js",
                        "~/App/Controllers/addFarmDialogCtrl.js",
                        "~/App/Controllers/editFarmsCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/farmmanagement/treatmentinfo")
                    .Include("~/App/Services/treatmentDataService.js",
                    "~/App/Controllers/addTreatmentDialogCtrl.js",
                    "~/App/Controllers/treatmentDataCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/authentication/edituserroles")
                    .Include("~/App/Services/editUserRolesService.js",
                    "~/App/Controllers/editUserRolesDialogCtrl.js",
                    "~/App/Controllers/userManagementCtrl.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/vendor/chartist", cdnJsChartist)
                    .Include("~/Scripts/chartist.js"));

            bundles
                .Add(new ScriptBundle("~/bundles/reporting/vendor/chartist-plugins")
                    .Include("~/Scripts/chartist-plugin-vertical-line.js",
                        "~/Scripts/chartist-plugin-tooltip.js",
                        "~/Scripts/chartist-plugin-axistitle.js"));

            bundles
                .Add(new StyleBundle("~/Content/css/angular")
                    .Include("~/Content/angular-material.css",
                        "~/Content/angular-material.layouts.css"));

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
                .Add(new StyleBundle("~/Content/css/authentication/common")
                    .Include("~/Content/authentication.css")); 
            
            bundles
                .Add(new StyleBundle("~/Content/css/farmManagement/common")
                    .Include("~/Content/farmManagement.css")); 
            
            bundles
                .Add(new StyleBundle("~/Content/css/reporting/common")
                    .Include("~/Content/reporting.css"));
            
            bundles
                .Add(new StyleBundle("~/Content/css/reporting/charts")
                    .Include("~/Content/charts.css"));

            bundles
                .Add(new StyleBundle("~/Content/css/reporting/vendor/chartist", cdnCssChartist)
                    .Include("~/Content/chartist.min.css"));

            bundles
                .Add(new StyleBundle("~/Content/css/reporting/tables")
                    .Include("~/Content/tables.css"));

            bundles
                .Add(new StyleBundle("~/Content/css/reporting/vendor/dataTables", cdnCssDataTables)
                    .Include("~/Content/jquery.dataTables.css"));
        }
    }
}
