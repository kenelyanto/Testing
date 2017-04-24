using System.Web;
using System.Web.Optimization;

namespace SPKPemilihanKaryawan
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/myapp").Include("~/Assets/global/MyApp.js"));
            bundles.Add(new ScriptBundle("~/bundles/dependencyDropdown").Include("~/Assets/global/dependency-dropdown.js"));
            bundles.Add(new StyleBundle("~/bundles/metronic/styles/global-mandatory").Include(
                                                "~/Assets/global/plugins/font-awesome/css/font-awesome.min.css",
                                                "~/Assets/global/plugins/simple-line-icons/simple-line-icons.min.css",
                                                "~/Assets/global/plugins/bootstrap/css/bootstrap.min.css",
                                                "~/Assets/global/plugins/uniform/css/uniform.default.css",
                                                "~/Assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css"));

            bundles.Add(new StyleBundle("~/bundles/metronic/styles/global-theme").Include(
                                                "~/Assets/global/css/components.min.css",
                                                "~/Assets/global/css/plugins.min.css"));

            bundles.Add(new StyleBundle("~/bundles/metronic/styles/theme-layout").Include(
                                                "~/Assets/layouts/layout4/css/layout.min.css",
                                                "~/Assets/layouts/layout4/css/themes/light.min.css",
                                                "~/Assets/layouts/layout4/css/custom.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/core-plugin").Include(
                                                "~/Assets/global/plugins/jquery.min.js",
                                                "~/Assets/global/plugins/bootstrap/js/bootstrap.min.js",
                                                "~/Assets/global/plugins/js.cookie.min.js",
                                                "~/Assets/global/plugins/bootstrap-hover-dropdown/bootstrap-hover-dropdown.min.js",
                                                "~/Assets/global/plugins/jquery-slimscroll/jquery.slimscroll.min.js",
                                                "~/Assets/global/plugins/jquery.blockui.min.js",
                                                "~/Assets/global/plugins/uniform/jquery.uniform.min.js",
                                                "~/Assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js",
                                                "~/Assets/global/plugins/bootbox/bootbox.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/theme-global").Include(
                                                "~/Assets/global/scripts/app.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/theme-layout").Include(
                                                "~/Assets/layouts/layout4/scripts/layout.min.js",
                                                "~/Assets/layouts/layout4/scripts/demo.min.js",
                                                "~/Assets/layouts/global/scripts/quick-sidebar.min.js"));

            // Select2 plugin
            bundles.Add(new StyleBundle("~/bundles/metronic/styles/plugin-select2").Include(
                                                "~/Assets/global/plugins/select2/css/select2.min.css",
                                                "~/Assets/global/plugins/select2/css/select2-bootstrap.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/plugin-select2").Include(
                                                "~/Assets/global/plugins/select2/js/select2.full.min.js"));


            //Datetimepicker plugin
            bundles.Add(new StyleBundle("~/bundles/metronic/styles/plugin-datetimepicker").Include(
                                                "~/Assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.css",
                                                "~/Assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css",
                                                "~/Assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css",
                                                "~/Assets/global/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css",
                                                "~/Assets/global/plugins/clockface/css/clockface.css"));
            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/plugin-datetimepicker").Include(
                                                "~/Assets/global/plugins/moment.min.js",
                                                "~/Assets/global/plugins/bootstrap-daterangepicker/daterangepicker.min.js",
                                                "~/Assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js",
                                                "~/Assets/global/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js",
                                                "~/Assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js",
                                                "~/Assets/global/plugins/clockface/js/clockface.js"));
            //Tagsinput plugin
            bundles.Add(new StyleBundle("~/bundles/metronic/styles/plugin-tagsinput").Include(
                                                "~/Assets/global/plugins/bootstrap-tagsinput/bootstrap-tagsinput.css"));
            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/plugin-tagsinput").Include(
                                                "~/Assets/pages/scripts/components-bootstrap-tagsinput.min.js",
                                                "~/Assets/global/plugins/bootstrap-tagsinput/bootstrap-tagsinput.min.js"));

            // jQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/plugin-jqueryvalidation").Include(
                                    "~/Assets/global/plugins/jquery-validation/js/jquery.validate.min.js",
                                    "~/Assets/global/plugins/jquery-validation/js/additional-methods.min.js"));

            //Datatables
            bundles.Add(new StyleBundle("~/bundles/metronic/styles/plugin-datatables").Include(
                                                "~/Assets/global/plugins/datatables/datatables.min.css",
                                                "~/Assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.css"));
            bundles.Add(new ScriptBundle("~/bundles/metronic/scripts/plugin-datatables").Include(
                                    "~/Assets/global/scripts/datatable.js",
                                    "~/Assets/global/plugins/datatables/datatables.min.js",
                                    "~/Assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/uicontrol").Include("~/Assets/global/UIControl.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/Site.css"));
        }
    }
}
