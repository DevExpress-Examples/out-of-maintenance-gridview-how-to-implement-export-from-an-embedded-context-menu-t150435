using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using Export.Models;

namespace Export.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            if(Session["TypedListModel"] == null)
                Session["TypedListModel"] = InMemoryModel.GetTypedListModel();

            return View(Session["TypedListModel"]);
        }

        public ActionResult TypedListDataBindingPartial() {
            return PartialView(Session["TypedListModel"]);
        }

        public ActionResult ExportTo(string OutputFormat) {
            var model = Session["TypedListModel"];

            switch(OutputFormat.ToUpper()) {
                case "CSV":
                    return GridViewExtension.ExportToCsv(GridViewHelper.ExportGridViewSettings, model);
                case "PDF":
                    return GridViewExtension.ExportToPdf(GridViewHelper.ExportGridViewSettings, model);
                case "RTF":
                    return GridViewExtension.ExportToRtf(GridViewHelper.ExportGridViewSettings, model);
                case "XLS":
                    return GridViewExtension.ExportToXls(GridViewHelper.ExportGridViewSettings, model);
                case "XLSX":
                    return GridViewExtension.ExportToXlsx(GridViewHelper.ExportGridViewSettings, model);
                default:
                    return RedirectToAction("Index");
            }
        }

    }
}
public static class GridViewHelper {
    private static GridViewSettings exportGridViewSettings;
    
    public static GridViewSettings ExportGridViewSettings {
        get {
            if(exportGridViewSettings == null)
                exportGridViewSettings = CreateExportGridViewSettings();
            return exportGridViewSettings;
        }
    }

    private static void AddMenuSubItem(GridViewContextMenuItem parentItem, string text, string name, string imageUrl, bool isPostBack) {
        var exportToXlsItem = parentItem.Items.Add(text, name);
        exportToXlsItem.Image.Url = imageUrl;
    }
     
    private static GridViewSettings CreateExportGridViewSettings() {
        GridViewSettings settings = new GridViewSettings();

        settings.Name = "gvTypedListDataBinding";
        settings.CallbackRouteValues = new { Controller = "Home", Action = "TypedListDataBindingPartial" };

        settings.KeyFieldName = "ID";
        settings.Settings.ShowFilterRow = true;

        settings.Columns.Add("ID");
        settings.Columns.Add("Text");
        settings.Columns.Add("Quantity");
        settings.Columns.Add("Price");

        settings.SettingsContextMenu.Enabled = true;
        settings.SettingsContextMenu.RowMenuItemVisibility.DeleteRow = false;
        settings.SettingsContextMenu.RowMenuItemVisibility.EditRow = false;
        settings.SettingsContextMenu.RowMenuItemVisibility.NewRow = false;
        settings.FillContextMenuItems = (sender, e) =>
        {
            if (e.MenuType == GridViewContextMenuType.Rows) {
                var item = e.CreateItem("Export", "Export");
                item.BeginGroup = true;
                e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item);
                AddMenuSubItem(item, "PDF", "PDF", @"Images/ExportToPdf.png", true);
                AddMenuSubItem(item, "XLS", "XLS", @"Images/ExportToXls.png", true);
            }
        };

        settings.ClientSideEvents.ContextMenuItemClick = "ExportGridView";
        return settings;
    }
}