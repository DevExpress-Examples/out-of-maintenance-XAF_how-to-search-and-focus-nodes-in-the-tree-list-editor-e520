Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.Web.ASPxTreeList
Imports DevExpress.Persistent.Base.General
Imports DevExpress.ExpressApp.TreeListEditors.Web
Imports System.Web.UI.WebControls

Namespace E520.Module.Web.Controllers
    Public Class WebFilterTreeListViewController
        Inherits ObjectViewController(Of ListView, ITreeNode)

        Private treeList As ASPxTreeList
        Private findNodeAction As ParametrizedAction
        Public Sub New()
            findNodeAction = New ParametrizedAction(Me, "FindNode", DevExpress.Persistent.Base.PredefinedCategory.FullTextSearch, GetType(String))
            findNodeAction.Caption = "Smart Search"
            findNodeAction.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image
            AddHandler findNodeAction.Execute, AddressOf findNodeAction_Execute
        End Sub
        Protected Overrides Sub OnViewControlsCreated()
            MyBase.OnViewControlsCreated()
            Dim treeListEditor As ASPxTreeListEditor = TryCast(View.Editor, ASPxTreeListEditor)
            If treeListEditor IsNot Nothing Then
                treeList = treeListEditor.TreeList
                AddHandler treeList.HtmlDataCellPrepared, AddressOf treeList_HtmlDataCellPrepared
            End If
        End Sub
        Private Sub treeList_HtmlDataCellPrepared(ByVal sender As Object, ByVal e As TreeListHtmlDataCellEventArgs)
            Dim textToSearch As String = TryCast(findNodeAction.Value, String)
            If (Not String.IsNullOrEmpty(textToSearch)) AndAlso e.CellValue IsNot Nothing Then
                Dim propertyValue As String = e.CellValue.ToString()
                Dim textIndex As Integer = propertyValue.ToLower().IndexOf(textToSearch.ToLower())
                If textIndex >= 0 Then
                    Dim spanLength As Integer = ("<span class='highlight'>").Length
                    propertyValue = propertyValue.Insert(textIndex, "<span class='highlight'>")
                    propertyValue = propertyValue.Insert(textIndex + spanLength + textToSearch.Length, "</span>")
                    Dim label As New Label()
                    label.Text = propertyValue
                    e.Cell.Controls.Clear()
                    e.Cell.Controls.Add(label)
                End If
            End If
        End Sub
        Private Sub findNodeAction_Execute(ByVal sender As Object, ByVal e As ParametrizedActionExecuteEventArgs)
            Dim searchText As String = TryCast(e.ParameterCurrentValue, String)
            If Not String.IsNullOrEmpty(searchText) Then
                treeList.ExpandAll()
            End If
        End Sub
        Protected Overrides Sub OnDeactivated()
            MyBase.OnDeactivated()
            If treeList IsNot Nothing Then
                RemoveHandler treeList.HtmlDataCellPrepared, AddressOf treeList_HtmlDataCellPrepared
                treeList = Nothing
            End If
        End Sub
    End Class
End Namespace
