Imports System
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Persistent.BaseImpl

Namespace E520.Module.DatabaseUpdate

    ' For more typical usage scenarios, be sure to check out http://documentation.devexpress.com/#Xaf/clsDevExpressExpressAppUpdatingModuleUpdatertopic
    Public Class Updater
        Inherits ModuleUpdater

        Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
            MyBase.New(objectSpace, currentDBVersion)
        End Sub

        Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
            MyBase.UpdateDatabaseAfterUpdateSchema()
            Dim parent1 As HCategory = CreateNode("Parent 1", Nothing)
            CreateNode("Child 1", parent1)
            CreateNode("Child 2", parent1)
            CreateNode("Parent 2", Nothing)
        End Sub

        Private Function CreateNode(ByVal name As String, ByVal parent As HCategory) As HCategory
            Dim obj As HCategory = ObjectSpace.FindObject(Of HCategory)(New BinaryOperator("Name", name))
            If obj Is Nothing Then
                obj = ObjectSpace.CreateObject(Of HCategory)()
                obj.Name = name
                obj.Parent = parent
            End If

            Return obj
        End Function
    End Class
End Namespace
