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
        input = Mid(input, 1, input.Length - 2)

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
End Class
