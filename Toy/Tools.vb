Module Tools

    Public Function pluralize(word As String, count As Integer) As String
        If Math.Abs(count) = 1 Then
            Return word
        End If
        Return word & "s"
    End Function

End Module
