Module common
    '
    '
    '
    Public Function serializePackageToJson(ByVal CP As Contensive.BaseClasses.CPBaseClass, ByVal _Object As packageClass) As String
        Dim s As String = ""
        '
        Try
            s = Newtonsoft.Json.JsonConvert.SerializeObject(_Object).Replace(vbCrLf, "")
        Catch ex As Exception
            Try
                CP.Site.ErrorReport(ex, "error in Contensive.Addons.commonClass.serializePackageToJson")
            Catch errObj As Exception
            End Try
        End Try
        '
        Return s
    End Function
    '
    '=====================================================================================
    ' json packages
    '=====================================================================================
    '
    <Serializable()>
    Public Class packageClass
        Public success As Boolean = False
        Public errors As New List(Of packageError)
        Public dataSets As New List(Of packageDataSet)
        '
        Public Sub addError(ByVal CP As contensive.BaseClasses.CPBaseClass, ByVal errNumber As Integer, ByVal errDescription As String)
            Try
                Dim packageError As New packageError
                '
                packageError.number = errNumber
                packageError.description = errDescription
                '
                errors.Add(packageError)
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in packageClass.addError")
                Catch errObj As Exception
                End Try
            End Try
        End Sub
    End Class
    '
    '
    <Serializable()>
    Public Class packageDataSet
        Public dataFor As String = ""
        Public errors As New List(Of packageError)
        Public data As Object
        '
        Public Sub addError(ByVal CP As contensive.BaseClasses.CPBaseClass, ByVal errNumber As Integer, ByVal errDescription As String)
            Try
                Dim packageError As New packageError
                '
                packageError.number = errNumber
                packageError.description = errDescription
                '
                errors.Add(packageError)
            Catch ex As Exception
                Try
                    CP.Site.ErrorReport(ex, "error in packageClass.addError")
                Catch errObj As Exception
                End Try
            End Try
        End Sub
    End Class

    '
    '
    '
    <Serializable()>
    Public Class packageError
        Public number As Integer = 0
        Public description As String = ""
    End Class

End Module
