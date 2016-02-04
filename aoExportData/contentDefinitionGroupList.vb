Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace contensive.addon.aoExportData
    '
    '
    '
    Public Class contentDefinitionGroupListClass
        Inherits AddonBaseClass
        '
        '
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Dim package As New packageClass
            Dim packageData As New packageDataSet
            Try
                Dim treeDetailList As New List(Of oneGroupDetailClass)
                Dim oneTreeDetail As oneGroupDetailClass

                Dim cs As CPCSBaseClass = CP.CSNew()

                If cs.Open("Groups", , "Name") Then
                    Do
                        oneTreeDetail = New oneGroupDetailClass
                        '
                        oneTreeDetail.id = cs.GetInteger("id")
                        oneTreeDetail.name = cs.GetText("name")
                        '
                        treeDetailList.Add(oneTreeDetail)
                        '
                        Call cs.GoNext()
                    Loop While cs.OK

                End If
                Call cs.Close()

                packageData.dataFor = "Content Definition Group List"
                packageData.data = treeDetailList
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
        Private Class oneGroupDetailClass
            Public id As Integer
            Public name As String
            Public selected As Boolean
        End Class

    End Class
End Namespace
