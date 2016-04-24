Imports System.Text

Public Class Tuple

    Public values As New List(Of Object)
    Private parent As Table

    Public Sub New(parent As Table)
        Me.parent = parent
    End Sub

    Public Overrides Function ToString() As String
        Dim out As New StringBuilder
        For i As Integer = 0 To parent.numberOfAttributes - 1
            out.AppendLine(String.Format("{0}: {1}", parent.columns(i).name, values(i)))
        Next
        Return out.ToString
    End Function

    Public Overloads Function ToString(format As String) As String
        If format = "f" Then
            Dim out As New StringBuilder

            If values.Count = 0 Then
                Return "{}"
            End If

            out.Append("{")
            out.Append(escapeVal(values(0).ToString))
            For i As Integer = 1 To values.Count - 1
                out.Append("|")
                out.Append(escapeVal(values(i)))
            Next
            out.Append("}")
            Return out.ToString
        Else
            Return ToString()
        End If
    End Function

    Private Function escapeVal(val As Object) As String
        If TypeOf val Is Boolean Then
            Return If(val, "T", "F")
        ElseIf TypeOf val Is String Then
            Return CType(val, String).Replace("|", "||").Replace("{", "|{").Replace("}", "|}")
        End If
        Return val
    End Function

End Class
