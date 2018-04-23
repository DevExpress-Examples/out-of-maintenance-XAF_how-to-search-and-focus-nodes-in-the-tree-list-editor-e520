Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.XtraTreeList
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.ExpressApp.SystemModule
Imports DevExpress.ExpressApp.Win.Controls
Imports DevExpress.ExpressApp.TreeListEditors.Win

Namespace WinSolution.Module.Win
	Partial Public Class FilterTreeListViewController
		Inherits ViewController(Of ListView)
		Private treeList As TreeList
		Private startNode As TreeListNode
		Private currentNode As TreeListNode
		Private fullTextFilterAction As ParametrizedAction
		Public Sub New()
			TargetViewId = "TestTreeObject_ListView"
		End Sub
		Protected Overrides Sub OnViewControlsCreated()
			MyBase.OnViewControlsCreated()
			fullTextFilterAction = Frame.GetController(Of FilterController)().FullTextFilterAction
			AddHandler fullTextFilterAction.Executed, AddressOf FullTextFilterAction_Executed
			Dim treeListEditor As TreeListEditor = TryCast(View.Editor, TreeListEditor)
			If treeListEditor IsNot Nothing Then
				treeList = treeListEditor.TreeList
				treeList = treeListEditor.TreeList
				AddHandler treeList.KeyDown, AddressOf treeList_KeyDown
				AddHandler treeList.Disposed, AddressOf treeList_Disposed
			End If
		End Sub
		Private Sub FullTextFilterAction_Executed(ByVal sender As Object, ByVal e As ActionBaseEventArgs)
			If startNode Is Nothing Then
				startNode = treeList.Nodes.FirstNode
				currentNode = startNode
				treeList.FocusedNode = Nothing
			End If
			TreeListSearch(TryCast(fullTextFilterAction.Value, String))
		End Sub
		Private Sub treeList_Disposed(ByVal sender As Object, ByVal e As EventArgs)
			Dim treeList As TreeList = CType(sender, TreeList)
			RemoveHandler treeList.KeyDown, AddressOf treeList_KeyDown
			RemoveHandler treeList.Disposed, AddressOf treeList_Disposed
		End Sub
		Private Sub treeList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
			If e.KeyCode = System.Windows.Forms.Keys.F3 Then
				TreeListSearch(TryCast(fullTextFilterAction.Value, String))
			End If
		End Sub
		Protected Overridable Sub TreeListSearch(ByVal value As String)
			If (Not String.IsNullOrEmpty(value)) Then
				DoSearch(value)
			End If
		End Sub
		Protected Sub DoSearch(ByVal textToSearch As String)
			Do While currentNode IsNot Nothing
				Dim value As String = TryCast(currentNode(View.ObjectTypeInfo.DefaultMember.BindingName), String)
				If value IsNot Nothing Then
					If value.Contains(textToSearch) Then
						treeList.FocusedNode = currentNode
						currentNode = GetNextTreeListNode(currentNode)
						If currentNode Is Nothing Then
							currentNode = startNode
						End If
						Return
					End If
					currentNode = GetNextTreeListNode(currentNode)
					If currentNode Is Nothing Then
						currentNode = startNode
						Exit Do
					End If
				End If
			Loop
		End Sub
		Private Function GetNextTreeListNode(ByVal node As TreeListNode) As TreeListNode
			If node Is Nothing Then
				Return Nothing
			End If
			Dim onode As ObjectTreeListNode = TryCast(node, ObjectTreeListNode)
			If onode IsNot Nothing Then
				CType(treeList, ObjectTreeList).BuildChildNodes(onode)
			End If
			If node.Nodes.Count > 0 Then
				Return node.Nodes(0)
			End If
			If node.ParentNode IsNot Nothing Then
				Dim owner As TreeListNodes = node.ParentNode.Nodes
				Do While node Is owner.LastNode
					If owner Is treeList.Nodes Then
						Return Nothing
					End If
					If node.ParentNode Is Nothing Then
						Return Nothing
					End If
					node = node.ParentNode

					owner = If(node.ParentNode Is Nothing, treeList.Nodes, node.ParentNode.Nodes)
				Loop
				Dim index As Integer = owner.IndexOf(node)
				Return owner(index + 1)
			Else
				If treeList.Nodes.LastNode Is node Then
					Return Nothing
				Else
					Dim index As Integer = treeList.Nodes.IndexOf(node)
					Return treeList.Nodes(index + 1)
				End If
			End If
		End Function
		Protected Overrides Sub OnDeactivated()
			RemoveHandler fullTextFilterAction.Executed, AddressOf FullTextFilterAction_Executed
			fullTextFilterAction = Nothing
			treeList = Nothing
			currentNode = Nothing
			startNode = Nothing
			MyBase.OnDeactivated()
		End Sub
	End Class
End Namespace
