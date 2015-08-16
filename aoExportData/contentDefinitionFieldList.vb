Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace contensive.addon.aoExportData
    '
    ' 
    '
    Public Class contentDefinitionFieldListClass
        Inherits AddonBaseClass
        '
        '
        '
        Public Overrides Function Execute(ByVal CP As CPBaseClass) As Object
            Dim returnHtml As String
            Dim package As New packageClass
            Dim packageData As New packageDataSet
            Try
                '
                Dim treeDetailList As New List(Of oneTreeDetailClass)
                Dim oneTreeDetail As oneTreeDetailClass

                Dim contentID As Integer = CP.Doc.GetInteger("contentID")

                'CP.Utils.AppendLog("Loging.log", contentID.ToString)

                'contentID = 27

                Dim cs As CPCSBaseClass = CP.CSNew()
                Dim csContent As CPCSBaseClass = CP.CSNew()

                Dim LookupFieldTypeID As Integer = CP.Content.GetRecordID("Content Field Types", "Lookup")
                '
                Dim RedirectFieldTypeID As Integer = CP.Content.GetRecordID("Content Field Types", "Redirect")
                Dim TextFileFieldTypeID As Integer = CP.Content.GetRecordID("Content Field Types", "TextFile")
                Dim ManyToManyFieldTypeID As Integer = CP.Content.GetRecordID("Content Field Types", "ManyToMany")
                Dim MemberSelectFieldTypeID As Integer = CP.Content.GetRecordID("Content Field Types", "Member Select ")
                '
                Dim lookupContentID As Integer = 0

                If cs.Open("Content Fields", "ContentID = " & contentID, "EditSortPriority") Then
                    Do
                        If (cs.GetInteger("Type") <> RedirectFieldTypeID) And (cs.GetInteger("Type") <> TextFileFieldTypeID) And (cs.GetInteger("Type") <> ManyToManyFieldTypeID) And (cs.GetInteger("Type") <> MemberSelectFieldTypeID) Then
                            '
                            oneTreeDetail = New oneTreeDetailClass
                            '
                            oneTreeDetail.id = cs.GetInteger("id")
                            oneTreeDetail.name = cs.GetText("name")
                            oneTreeDetail.caption = cs.GetText("caption")
                            '
                            If cs.GetInteger("Type") = LookupFieldTypeID Then
                                oneTreeDetail.isLookup = True
                                lookupContentID = cs.GetInteger("LookupContentID")
                                'Content ContentTableID
                                If csContent.Open("Content", "id=" & lookupContentID) Then
                                    oneTreeDetail.contentName = csContent.GetText("ContentTableID")
                                End If
                                csContent.Close()
                            Else
                                oneTreeDetail.isLookup = False
                            End If
                            '
                            oneTreeDetail.selected = False
                            treeDetailList.Add(oneTreeDetail)
                            '
                        End If

                        Call cs.GoNext()
                    Loop While cs.OK
                End If


                Call cs.Close()

                packageData.dataFor = "Content Definition Field List"
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
        Private Class oneTreeDetailClass
            Public id As Integer
            Public name As String
            Public caption As String
            Public contentName As String
            Public selected As Boolean
            Public isLookup As Boolean
        End Class

    End Class
End Namespace
