﻿using System.Web;
using System.Web.Optimization;

namespace GraduateDesignBk
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery-2.0.3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                     "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js",
                     "~/Scripts/twitter-bootstrap-hover-dropdown.min.js",
                     "~/Scripts/bootstrap-admin-theme-change-size.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/bootstrap.min.css",
                       "~/Content/bootstrap-theme.min.css"));
            bundles.Add(new StyleBundle("~/Content/AdminCss").Include(
                   "~/Content/bootstrap-admin-theme.css",
                     "~/Content/bootstrap-admin-theme-change-size.css"));
        }
    }
}
