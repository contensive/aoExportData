Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace contensive.addon.aoExportData
    '
    '
    '
    Public Class contentDefinitionListClass
        Inherits AddonBaseClass
        '
        '
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Dim package As New packageClass
            Dim packageData As New packageDataSet

            Dim oneContent As contentDefinitionClass
            Dim contentList As New List(Of contentDefinitionClass)
            Try
                Dim cs As CPCSBaseClass = CP.CSNew()

                If cs.Open("Content", , "name asc") Then
                    Do

                        oneContent = New contentDefinitionClass
                        oneContent.id = cs.GetInteger("id")
                        oneContent.name = cs.GetText("name")
                        contentList.Add(oneContent)
                        Call cs.GoNext()
                    Loop While cs.OK
                End If
                cs.Close()

                packageData.dataFor = "Content Definition List"
                packageData.data = contentList
                package.dataSets.Add(packageData)

                package.success = True
                '
                returnHtml = serializePackageToJson(CP, package)
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                package.success = False
                package.addError(CP, 0, "Exception Error")
                returnHtml = serializePackageToJson(CP, package)
            End Try
            Return returnHtml
        End Function
        '
        '
        '
        Private Sub errorReport(ByVal cp As CPBaseClass, ByVal ex As Exception, ByVal method As String)
            Try
                cp.Site.ErrorReport(ex, "Unexpected error in sampleClass." & method)
            Catch exLost As Exception
                '
                ' stop anything thrown from cp errorReport
                '
            End Try
        End Sub
        '
        '
        '
        Private Class contentDefinitionClass
            Public id As Integer
            Public name As String
        End Class
    End Class
    '
    '
    '
End Namespace
