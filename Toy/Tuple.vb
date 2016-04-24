Imports System.Text
Imports Toy

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
        Do
            val = inStr.Read
            If val = -1 OrElse ChrW(val) = "|" Then
                If values.Count = parent.numberOfAttributes Then
                    Throw New Exception("Number of records does not match the table definition")
                End If
                values.Add(parent.columns(values.Count).valToObj(sb.ToString))
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
            Throw New Exception("Number of attributes does not match the table definition")
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

End Class

Public Class TupleList
    Inherits List(Of Tuple)

    Public Overrides Function ToString() As String
        Dim out As New StringBuilder
        For Each t As Tuple In Me
            out.AppendLine(t.ToString)
        Next
        Return out.ToString
    End Function

    Public Overloads Function FindAll(match As Predicate(Of Tuple)) As TupleList
        Dim o As New TupleList
        o.AddRange(MyBase.FindAll(match))
        Return o
    End Function

End Class