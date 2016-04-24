Module Toy

    Sub Main()
        'Dim c As New Column("Test", Column.dataType.integer)
        'Console.WriteLine(c.ToString)

        Dim ta As New Table("[3][test:4][thsa:1][oen:3][1]" & vbCrLf &
                            "{test 1|10|F}")
        'ta.columns.Add(New Column("Col1", Column.dataType.string))
        'ta.columns.Add(New Column("Col2", Column.dataType.integer))
        'ta.columns.Add(New Column("Col3", Column.dataType.boolean))

        'Dim t As New Tuple(ta)
        't.values.Add("test")
        't.values.Add(50)
        't.values.Add(True)

        'ta.records.Add(t)

        'Console.Write(ta.ToString("f"))

        Dim t As New Tuple(ta, "{test|-10|T}")
        ta.records.Add(t)
        Console.WriteLine(ta.ToString(""))

        Console.ReadKey()
    End Sub

End Module
