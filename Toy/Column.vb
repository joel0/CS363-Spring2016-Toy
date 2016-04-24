Imports Toy

Public Class Column
    Public Enum dataType
        [integer] = 1
        [double] = 2
        [boolean] = 3
        [string] = 4
    End Enum

    Public name As String
    Public type As dataType

    Public Sub New(name As String, type As dataType)
        Me.name = name
        Me.type = type
    End Sub

    Public Sub New(input As String)
        ' Trim [ and ]
        If input.First <> "["c Or input.Last <> "]"c Then
            Throw New Exception("[ and ] expected on column")
        End If
        input = Mid(input, 2, input.Length - 2)

        ' Split on :
        Dim parts() As String = input.Split(":")
        If parts.Count <> 2 Then
            Throw New Exception("Column definition must contain one :")
        End If

        name = parts(0)
        If Not Integer.TryParse(parts(1), type) Then
            Throw New Exception("Column type must be an integer")
        End If
        If type > 4 Or type < 1 Then
            Throw New Exception("Column type out of range")
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("{0}, {1}", name, type)
    End Function

    Public Overloads Function ToString(format As String) As String
        If format = "f" Then ' File
            Return String.Format("[{0}:{1}]", name, CType(type, Integer))
        Else
            Return ToString()
        End If
    End Function

    Public Function valToObj(val As String) As Object
        Dim out As Object
        Select Case type
            Case Column.dataType.integer
                out = New Integer
                If Not Integer.TryParse(val, out) Then
                    Throw New Exception("Numeric value expected")
                End If
            Case Column.dataType.double
                out = New Double
                If Not Double.TryParse(val, out) Then
                    Throw New Exception("Double value expected")
                End If
            Case Column.dataType.boolean
                If val = "T" Then
                    out = True
                ElseIf val = "F" Then
                    out = False
                Else
                    Throw New Exception("T or F expected as boolean")
                End If
            Case Column.dataType.string
                out = val
            Case Else
                out = val
        End Select
        Return out
    End Function

End Class

Public Class ColumnList
    Inherits ObjectModel.Collection(Of Column)

    Protected Overrides Sub InsertItem(index As Integer, item As Column)
        If Me.Any(Function(c As Column) c.name = item.name) Then
            Throw New Exception("Duplicate column name")
        End If
        MyBase.InsertItem(index, item)
    End Sub
End Class