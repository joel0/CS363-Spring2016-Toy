Module Toy

    Sub Main()
        'Dim c As New Column("Test", Column.dataType.integer)
        'Console.WriteLine(c.ToString)

        Dim ta As New Table
        ta.columns.Add(New Column("Col1", Column.dataType.string))
        ta.columns.Add(New Column("Col2", Column.dataType.integer))
        ta.columns.Add(New Column("Col3", Column.dataType.boolean))

        'Dim t As New Tuple(ta)
        't.values.Add("test")
        't.values.Add(50)
        't.values.Add(True)

        'ta.records.Add(t)

        'Console.Write(ta.ToString("f"))

        Dim t As New Tuple(ta, "{test|-10|T}")
        Console.WriteLine(t)

        Console.ReadKey()
    End Sub

End Module
