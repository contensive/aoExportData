Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Contensive.BaseClasses

Namespace contensive.addon.aoExportData
    '
    '
    '
    Public Class contentDefinitionFieldProcessClass
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
                Dim csContent As CPCSBaseClass = CP.CSNew()
                Dim csLookup As CPCSBaseClass = CP.CSNew()
                Dim csTasks As CPCSBaseClass = CP.CSNew()
                Dim treeDetailList As New List(Of oneTreeDetailClass)
                Dim contentID As Integer = CP.Doc.GetInteger("contentID")
                '
                Dim mainSQLTable As String = String.Empty
                Dim SelectSQL As String = String.Empty
                Dim FromSQL As String = String.Empty
                Dim finalSQL As String = String.Empty


                Dim LookupTableNAMESQL As String = "LookupTable"
                Dim actualLookupName As String = String.Empty
                Dim actualLookup As Integer = 0
                '
                Dim lookupList As String = String.Empty

                Dim fileName As String = String.Empty

                treeDetailList = deserializeTreeDetailList(CP, CP.Doc.GetText("jsonData"))

                If contentID <> 0 Then
                    If csContent.Open("Content", " id = " & contentID) Then
                        mainSQLTable = csContent.GetText("ContentTableID")

                        ' Loop inside the Array Field
                        For Each oneField As oneTreeDetailClass In treeDetailList
                            If oneField.selected Then

                                If oneField.isLookup Then

                                    ' Exist 2 Types of lookups

                                    If String.IsNullOrEmpty(oneField.contentName) Then
                                        'is a empty field
                                        ' Lookup its a field
                                        SelectSQL &= ", " & mainSQLTable & "." & oneField.name

                                    Else
                                        ' lookup to a content definition
                                        ' create the lookup alias
                                        actualLookup += 1
                                        actualLookupName = LookupTableNAMESQL & actualLookup.ToString
                                        ' create the select field
                                        SelectSQL &= ", " & actualLookupName & ".name as " & oneField.name

                                        If actualLookup = 1 Then
                                            FromSQL = " (" & mainSQLTable & " LEFT JOIN " & oneField.contentName & " AS " & actualLookupName & " ON " & mainSQLTable & "." & oneField.name & " = " & actualLookupName & ".ID ) "
                                        Else
                                            FromSQL = " (" & FromSQL & " LEFT JOIN " & oneField.contentName & " AS " & actualLookupName & " ON " & mainSQLTable & "." & oneField.name & " = " & actualLookupName & ".ID ) "
                                        End If
                                    End If
                                Else
                                    ' create the select field
                                    SelectSQL &= ", " & mainSQLTable & "." & oneField.name
                                End If

                            End If
                        Next

                        If SelectSQL.Substring(0, 2) = ", " Then
                            SelectSQL = SelectSQL.Substring(2)
                        End If

                        If actualLookup = 0 Then
                            FromSQL = " " & mainSQLTable & " "
                        End If

                        finalSQL = "select " & SelectSQL & vbCrLf _
                            & " from " & FromSQL

                        CP.Utils.AppendLog("DBQuery.log", "Sql : " & finalSQL)

                        ' Create the report task
                        If csTasks.Insert("Tasks") Then
                            csTasks.SetField("Name", "Task " & csTasks.GetInteger("id") & " - CSV Download, Custom Report  at " & Now.ToString)
                            csTasks.SetField("Command", "BuildCSV")
                            csTasks.SetField("SQLQuery", finalSQL)
                            csTasks.SetField("Filename", csTasks.GetFilename("Filename", "export.csv"))
                        End If
                        csTasks.Close()

                    End If
                    Call csContent.Close()
                End If


                returnHtml = "OK"
            Catch ex As Exception
                errorReport(CP, ex, "execute")
                returnHtml = "Error"
            End Try
            Return returnHtml
        End Function
        '
        '
        '
        Private Function deserializeTreeDetailList(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal value As String) As List(Of oneTreeDetailClass)
            Dim o As New List(Of oneTreeDetailClass)
            '
            Try
                '
                '   for example see testDataSerializerClass
                '
                o = Newtonsoft.Json.JsonConvert.DeserializeObject(Of List(Of oneTreeDetailClass))(value)
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in contensive.addon.dli.GarmentAnalysisAppBackEndApi.deserializePaymentSubmission")
                Catch errObj As Exception
                End Try
            End Try
            '
            Return o
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
