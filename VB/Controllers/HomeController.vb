Imports System
Imports System.Collections.Generic
Imports System.Web
Imports System.Web.Mvc
Imports DevExpress.Web
Imports DevExpress.Web.Mvc
Imports Export.Models

Namespace Export.Controllers
	Public Class HomeController
		Inherits Controller

		Public Function Index() As ActionResult
			If Session("TypedListModel") Is Nothing Then
				Session("TypedListModel") = InMemoryModel.GetTypedListModel()
			End If

			Return View(Session("TypedListModel"))
		End Function

		Public Function TypedListDataBindingPartial() As ActionResult
			Return PartialView(Session("TypedListModel"))
		End Function

		Public Function ExportTo(ByVal OutputFormat As String) As ActionResult
			Dim model = Session("TypedListModel")

			Select Case OutputFormat.ToUpper()
				Case "CSV"
					Return GridViewExtension.ExportToCsv(GridViewHelper.ExportGridViewSettings, model)
				Case "PDF"
					Return GridViewExtension.ExportToPdf(GridViewHelper.ExportGridViewSettings, model)
				Case "RTF"
					Return GridViewExtension.ExportToRtf(GridViewHelper.ExportGridViewSettings, model)
				Case "XLS"
					Return GridViewExtension.ExportToXls(GridViewHelper.ExportGridViewSettings, model)
				Case "XLSX"
					Return GridViewExtension.ExportToXlsx(GridViewHelper.ExportGridViewSettings, model)
				Case Else
					Return RedirectToAction("Index")
			End Select
		End Function

	End Class
End Namespace
Public NotInheritable Class GridViewHelper

	Private Sub New()
	End Sub
'INSTANT VB NOTE: The variable exportGridViewSettings was renamed since Visual Basic does not allow variables and other class members to have the same name:
	Private Shared exportGridViewSettings_Renamed As GridViewSettings

	Public Shared ReadOnly Property ExportGridViewSettings() As GridViewSettings
		Get
			If exportGridViewSettings_Renamed Is Nothing Then
				exportGridViewSettings_Renamed = CreateExportGridViewSettings()
			End If
			Return exportGridViewSettings_Renamed
		End Get
	End Property

	Private Shared Sub AddMenuSubItem(ByVal parentItem As GridViewContextMenuItem, ByVal text As String, ByVal name As String, ByVal imageUrl As String, ByVal isPostBack As Boolean)
		Dim exportToXlsItem = parentItem.Items.Add(text, name)
		exportToXlsItem.Image.Url = imageUrl
	End Sub

	Private Shared Function CreateExportGridViewSettings() As GridViewSettings
		Dim settings As New GridViewSettings()

		settings.Name = "gvTypedListDataBinding"
		settings.CallbackRouteValues = New With {Key .Controller = "Home", Key .Action = "TypedListDataBindingPartial"}

		settings.KeyFieldName = "ID"
		settings.Settings.ShowFilterRow = True

		settings.Columns.Add("ID")
		settings.Columns.Add("Text")
		settings.Columns.Add("Quantity")
		settings.Columns.Add("Price")

        settings.SettingsContextMenu.Enabled = True

        settings.SettingsContextMenu.RowMenuItemVisibility.DeleteRow = False
        settings.SettingsContextMenu.RowMenuItemVisibility.EditRow = False
        settings.SettingsContextMenu.RowMenuItemVisibility.NewRow = False

        settings.FillContextMenuItems = Sub(sender, e)
                                                  If e.MenuType = GridViewContextMenuType.Rows Then
                                                      Dim item = e.CreateItem("Export", "Export")
                                                      item.BeginGroup = True
                                                      e.Items.Insert(e.Items.IndexOfCommand(GridViewContextMenuCommand.Refresh), item)
                                                      AddMenuSubItem(item, "PDF", "PDF", "Images/ExportToPdf.png", True)
                                                      AddMenuSubItem(item, "XLS", "XLS", "Images/ExportToXls.png", True)
                                                  End If
                                              End Sub

		settings.ClientSideEvents.ContextMenuItemClick = "ExportGridView"
		Return settings
	End Function
End Class