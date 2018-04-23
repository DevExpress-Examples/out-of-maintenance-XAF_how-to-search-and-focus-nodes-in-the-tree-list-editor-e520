Imports Microsoft.VisualBasic
Imports System
Namespace WinSolution.Module.Win
	Partial Public Class FilterTreeListViewController
		''' <summary>
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary>
		''' Required method for Designer support - do not modify
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Me.parametrizedAction1 = New DevExpress.ExpressApp.Actions.ParametrizedAction(Me.components)
			' 
			' parametrizedAction1
			' 
			Me.parametrizedAction1.Caption = "MySearch"
			Me.parametrizedAction1.Id = "ae9d7059-b341-46d9-aa1a-63673a0662e0"
'			Me.parametrizedAction1.Execute += New DevExpress.ExpressApp.Actions.ParametrizedActionExecuteEventHandler(Me.parametrizedAction1_Execute);

		End Sub

		#End Region

		Private WithEvents parametrizedAction1 As DevExpress.ExpressApp.Actions.ParametrizedAction
	End Class
End Namespace
