Imports System.Text

Public Class Table

    Public ReadOnly Property numberOfAttributes As Integer
        Get
            Return columns.Count
        End Get
    End Property
    Public ReadOnly Property numberOfRecords As Integer
        Get
            Return records.Count
        End Get
    End Property
    Public columns As New List(Of Column)
    Public records As New List(Of Tuple)

    Public Overrides Function ToString() As String
        Dim out As New StringBuilder
        out.AppendLine(String.Format("{0} {1}", numberOfAttributes, Tools.pluralize("attribute", numberOfAttributes)))
        For i As Integer = 0 To numberOfAttributes - 1
            out.AppendLine(String.Format("Attribute {0}: {1}", i, columns(i)))
        Next
        out.Append(String.Format("{0} {1}", numberOfRecords, Tools.pluralize("record", numberOfRecords)))
        Return out.ToString
    End Function

    Public Overloads Function ToString(format As String) As String
        If format = "f" Then
            Dim out As New StringBuilder
            out.Append(String.Format("[{0}]", numberOfAttributes))
            For Each a As Column In columns
                out.Append(a.ToString("f"))
            Next
            out.AppendLine(String.Format("[{0}]", numberOfRecords))

            For Each r As Tuple In records
                out.AppendLine(r.ToString("f"))
            Next
            Return out.ToString
        Else
            Return ToString()
        End If
    End Function

End Class
