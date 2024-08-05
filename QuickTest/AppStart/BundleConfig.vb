Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Optimization

Public Class BundleConfig
    ' For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254726
    Public Shared Sub RegisterBundles(ByVal bundles As BundleCollection)
        bundles.Add(New ScriptBundle("~/bundles/WebFormsJs").Include(
                        "~/Scripts/WebForms/WebForms.js",
                        "~/Scripts/WebForms/WebUIValidation.js",
                        "~/Scripts/WebForms/MenuStandards.js",
                        "~/Scripts/WebForms/Focus.js",
                        "~/Scripts/WebForms/GridView.js",
                        "~/Scripts/WebForms/DetailsView.js",
                        "~/Scripts/WebForms/TreeView.js",
                        "~/Scripts/WebForms/WebParts.js"))

        ' Order is very important for these files to work, they have explicit dependencies
        bundles.Add(New ScriptBundle("~/bundles/MsAjaxJs").Include(
                "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"))

        ' Use the Development version of Modernizr to develop with and learn from. Then, when you’re
        ' ready for production, use the build tool at http://modernizr.com to pick only the tests you need
        bundles.Add(New ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"))

        'ActivityPage_Pad
        bundles.Add(New ScriptBundle("~/bundles/ActivityPage_Pad").Include(
                    "~/js/jquery-1.7.1.min.js",
                    "~/js/jquery-ui-1.8.18.js",
                    "~/js/jquery.blockUI.js",
                    "~/Scripts/jquery.signalR-2.0.2.min.js"
                    ))

        'ActivityPage_Pad2
        bundles.Add(New ScriptBundle("~/bundles/ActivityPage_Pad2").Include(
                    "~/js/jquery-1.7.1.js",
                    "~/js/jquery-ui-1.8.18.js",
                    "~/js/jquery.ui.touch-punch.min.js",
                    "~/js/jquery.blockUI.js",
                    "~/js/jquery.prettyPhoto.js",
                    "~/js/slides.min.jquery.js"
                    ))

    End Sub
End Class
