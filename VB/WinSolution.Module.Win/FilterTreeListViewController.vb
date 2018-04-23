Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.XtraTreeList.Nodes
Imports DevExpress.ExpressApp.TreeListEditors.Win
Imports DevExpress.XtraTreeList
Imports DevExpress.ExpressApp.Win.Controls

Namespace WinSolution.Module.Win
	Partial Public Class FilterTreeListViewController
		Inherits ViewController
		Public Sub New()
			InitializeComponent()
			RegisterActions(components)
			TargetViewId = "TestTreeObject_ListView"
		End Sub
		Protected Overrides Overloads Sub OnActivated()
			MyBase.OnActivated()
			AddHandler View.ControlsCreated, AddressOf View_ControlsCreated
		End Sub
		Private Sub View_ControlsCreated(ByVal sender As Object, ByVal e As EventArgs)
			Dim treeListEditor As TreeListEditor = CType((CType(View, ListView)).Editor, TreeListEditor)
			treeList = treeListEditor.TreeList
			AddHandler treeList.KeyDown, AddressOf treeList_KeyDown
		End Sub

		Private Sub treeList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
			If e.KeyCode = System.Windows.Forms.Keys.F3 Then
				Search(TryCast(parametrizedAction1.Value, String))
			End If
		End Sub
		Private Sub Search(ByVal value As String)
			If (Not String.IsNullOrEmpty(value)) Then
				DoSearchInternal(value)
			End If
		End Sub
		Private Sub parametrizedAction1_Execute(ByVal sender As Object, ByVal e As ParametrizedActionExecuteEventArgs) Handles parametrizedAction1.Execute
			If startNode Is Nothing Then
				startNode = treeList.Nodes.FirstNode
				currentNode = startNode
				treeList.FocusedNode = Nothing
			End If
			Search(TryCast(e.ParameterCurrentValue, String))
		End Sub
		Private treeList As TreeList
		Private startNode As TreeListNode
		Private currentNode As TreeListNode
		Public Sub DoSearchInternal(ByVal textToSearch As String)
			Do While currentNode IsNot Nothing
				Dim value As String = CStr(currentNode("Name"))
				If value.Contains(textToSearch) Then
					treeList.FocusedNode = currentNode
					currentNode = GetNextNode(currentNode)
					If currentNode Is Nothing Then
						currentNode = startNode
					End If
					Return
				End If
				currentNode = GetNextNode(currentNode)
				If currentNode Is Nothing Then
					currentNode = startNode
					Exit Do
				End If
			Loop
		End Sub
		Protected Function GetNextNode(ByVal node As TreeListNode) As TreeListNode
			If node Is Nothing Then
				Return Nothing
			End If
			Dim onode As ObjectTreeListNode = TryCast(node, ObjectTreeListNode)
			If onode IsNot Nothing Then
				CType(onode.TreeList, ObjectTreeList).BuildChildNodes(onode)
			End If
			If node.Nodes.Count > 0 Then
				Return node.Nodes(0)
			End If
			Dim treeList As TreeList = node.TreeList
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

					If node.ParentNode Is Nothing Then
						owner = treeList.Nodes
					Else
						owner = node.ParentNode.Nodes
					End If
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
	End Class
End Namespace
