Module Toy

    Sub Main()
        Dim args As ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs

        If args.Count < 2 Then
            showUsage()
            Return
        End If

        If args(0).Equals("create", StringComparison.CurrentCultureIgnoreCase) Then
            createTableInteractive(args(1))
        ElseIf args(0).Equals("header") Then
            showHeader(args(1))
        ElseIf args(0).Equals("insert", StringComparison.CurrentCultureIgnoreCase) Then
            insertInteractive(args(1))
        Else
            If args.Count >= 3 Then
                If args(0).Equals("display", StringComparison.CurrentCultureIgnoreCase) Then
                    Dim rid As Integer
                    If Not Integer.TryParse(args(1), rid) Then
                        Throw New Exception("rid must be an integer")
                    End If
                    displayRecord(args(2), rid)
                ElseIf args(0).Equals("delete", StringComparison.CurrentCultureIgnoreCase) Then
                    Dim rid As Integer
                    If Not Integer.TryParse(args(1), rid) Then
                        Throw New Exception("rid must be an integer")
                    End If
                    deleteRecord(args(2), rid)
                ElseIf args(0).Equals("search", StringComparison.CurrentCultureIgnoreCase) Then
                    search(args(2), args(1))
                End If
            Else
                showUsage()
                Return
            End If
        End If

        Console.ReadKey()
    End Sub

    Private Sub showUsage()
        Console.WriteLine("Usage")
    End Sub

    Private Sub createTableInteractive(fileName As String)

    End Sub

    Private Sub showHeader(fileName As String)

    End Sub

    Private Sub insertInteractive(fileName As String)

    End Sub

    Private Sub displayRecord(fileName As String, rid As Integer)

    End Sub

    Private Sub deleteRecord(fileName As String, rid As Integer)

    End Sub

    Private Sub search(fileName As String, condition As String)

    End Sub

End Module
