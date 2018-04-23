Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.XtraTreeList
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.ExpressApp.TreeListEditors.Win
Imports DevExpress.ExpressApp.DC
Imports DevExpress.Persistent.Base.General
Imports DevExpress.Data.Filtering

Namespace E520.Module.Win.Controllers
    Public Class WinFilterTreeListViewController
        Inherits ObjectViewController(Of ListView, ITreeNode)

        Private treeList As TreeList
        Public Sub New()
            Dim findNodeAction As New ParametrizedAction(Me, "FindNode", DevExpress.Persistent.Base.PredefinedCategory.FullTextSearch, GetType(String))
            findNodeAction.Caption = "Smart Search"
            AddHandler findNodeAction.Execute, AddressOf findNodeAction_Execute
            Dim focusNodeAction As New ParametrizedAction(Me, "FocusNode", DevExpress.Persistent.Base.PredefinedCategory.FullTextSearch, GetType(String))
            focusNodeAction.Caption = "Focus"
            AddHandler focusNodeAction.Execute, AddressOf focusNodeAction_Execute
        End Sub
        Protected Overrides Sub OnViewControlsCreated()
            MyBase.OnViewControlsCreated()
            Dim treeListEditor As TreeListEditor = TryCast(View.Editor, TreeListEditor)
            If treeListEditor IsNot Nothing Then
                treeList = treeListEditor.TreeList
                treeList.OptionsBehavior.EnableFiltering = True
                treeList.OptionsFilter.FilterMode = FilterMode.Extended
            End If
        End Sub
        Private Sub focusNodeAction_Execute(ByVal sender As Object, ByVal e As ParametrizedActionExecuteEventArgs)
            Dim textToSearch As String = TryCast(e.ParameterCurrentValue, String)
            Dim matchingNode As TreeListNode = Nothing
            If Not String.IsNullOrEmpty(textToSearch) Then
                matchingNode = treeList.FindNode(Function(node)
                    Dim currentObject As Object = node.Tag
                    Dim currentObjectTypeInfo As ITypeInfo = XafTypesInfo.Instance.FindTypeInfo(currentObject.GetType())
                    Dim value As String = TryCast(currentObjectTypeInfo.DefaultMember.GetValue(currentObject), String)
                    If value IsNot Nothing AndAlso value.ToLower().Contains(textToSearch.ToLower()) Then
                        Return True
                    Else
                        node.Expanded = True
                        Return False
                    End If
                End Function)
            End If
            treeList.FocusedNode = If(matchingNode, treeList.Nodes.FirstNode)
        End Sub
        Private Sub findNodeAction_Execute(ByVal sender As Object, ByVal e As ParametrizedActionExecuteEventArgs)
            Dim value As String = TryCast(e.ParameterCurrentValue, String)
            If Not String.IsNullOrEmpty(value) Then
                treeList.ExpandAll()
            End If
            treeList.ApplyFindFilter(value)
            ' Alternatively, use the TreeList.ActiveFilterCriteria property
            ' treeList.ActiveFilterCriteria = new DevExpress.Data.Filtering.FunctionOperator(FunctionOperatorType.Contains, new OperandProperty("Name"), new OperandValue(value)); 
        End Sub

        Protected Overrides Sub OnDeactivated()
            treeList = Nothing
            MyBase.OnDeactivated()
        End Sub
    End Class
End Namespace
