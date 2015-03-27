using System.Web;
using System.Web.Optimization;

namespace Annytab.Webshop
{
    /// <summary>
    /// This classs handles the configuration of bundles
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register bundles
        /// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        /// </summary>
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Add references to jquery script
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js",
                        "~/Scripts/jquery.mousewheel.js",
                        "~/Scripts/spin.min.js",
                        "~/Scripts/jquery.spin.js",
                        "~/Scripts/annytab.spin-image-360.js"));

            // Add references to annytab back-end scripts
            bundles.Add(new ScriptBundle("~/bundles/annytab_admin").Include(
                        "~/Scripts/annytab_admin/annytab.default.js"));

            // Add references to admin css files
            bundles.Add(new StyleBundle("~/Content/admin_css").Include(
                "~/Content/annytab_css/admin_default.css",
                "~/Content/annytab_css/admin_layout.css"));

        } // End of the RegisterBundles method

    } // End of the class

} // End of the namespace