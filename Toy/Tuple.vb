Imports System.Text

Public Class Tuple

    Public values As New List(Of Object)
    Private parent As Table

    Public Sub New(parent As Table)
        Me.parent = parent
    End Sub

    Public Sub New(parent As Table, input As String)
        Me.New(parent)

        If input.First <> "{"c Or input.Last <> "}"c Then
            Throw New Exception("Tuple must start with { and end with }")
        End If
        input = Mid(input, 2, input.Length - 2)

        Dim inStr As New IO.StringReader(input)
        Dim val As Integer
        Dim sb As New StringBuilder
        Dim i As Integer = 0
        Do
            val = inStr.Read
            If val = -1 OrElse ChrW(val) = "|" Then
                If i = parent.numberOfAttributes Then
                    Throw New Exception("Number of records does not match the table definition")
                End If
                values.Add(valToObj(sb.ToString, i))
                i += 1
                sb.Clear()
            Else
                If ChrW(val) = "\"c Then
                    Select Case ChrW(inStr.Peek)
                        Case "|"c
                            ' Escaped |
                            val = inStr.Read()
                        Case "{"c
                            ' Escaped {
                            val = inStr.Read()
                        Case "}"c
                            ' Escaped }
                            val = inStr.Read()
                        Case "\"
                            ' Escaped \
                            val = inStr.Read()
                    End Select
                End If
                sb.Append(ChrW(val))
            End If
        Loop While val <> -1
        If values.Count <> parent.numberOfAttributes Then
            Throw New Exception("Number of records does not match the table definition")
        End If
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
            Return CType(val, String).Replace("\", "\\").Replace("|", "\|").Replace("{", "\{").Replace("}", "\}")
        End If
        Return val
    End Function

    Private Function valToObj(val As String, index As Integer) As Object
        Dim out As Object
        Select Case parent.columns(index).type
            Case Column.dataType.integer
                out = New Integer
                If Not Integer.TryParse(val, out) Then
                    Throw New Exception("Numeric value expected in tuple")
                End If
            Case Column.dataType.double
                out = New Double
                If Not Double.TryParse(val, out) Then
                    Throw New Exception("Double value expected in tuple")
                End If
            Case Column.dataType.boolean
                If val = "T" Then
                    out = True
                ElseIf val = "F" Then
                    out = False
                Else
                    Throw New Exception("T or F expected as boolean in tuple")
                End If
            Case Column.dataType.string
                out = val
        End Select
        Return out
    End Function

End Class
