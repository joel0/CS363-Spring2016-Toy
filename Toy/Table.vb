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
    Public columns As New ColumnList
    Public records As New TupleList

    Public Sub New()
    End Sub

    Public Sub New(input As String)
        Dim reader As New IO.StringReader(input)
        Dim line As String
        Dim expectedAttributes As Integer
        Dim expectedRecords As Integer
        Dim headerLen As Integer = 0

        line = reader.ReadLine
        Dim matches As RegularExpressions.MatchCollection = RegularExpressions.Regex.Matches(line, "\[(?<val>[\w\d:]*)\]")
        If Not Integer.TryParse(matches(0).Groups("val").Value, expectedAttributes) Then
            Throw New Exception("Attribute count must be an integer")
        End If
        If Not Integer.TryParse(matches(matches.Count - 1).Groups("val").Value, expectedRecords) Then
            Throw New Exception("Record count must be an integer")
        End If
        headerLen += matches(0).Length + matches(matches.Count - 1).Length
        For i As Integer = 1 To matches.Count - 2
            columns.Add(New Column(matches(i).Value))
            headerLen += matches(i).Length
        Next
        If headerLen <> line.Length Then
            Throw New Exception("Junk in header")
        End If

        If numberOfAttributes <> expectedAttributes Then
            Throw New Exception("Mismatch of attributes to header value")
        End If

        line = reader.ReadLine()
        Do While line IsNot Nothing
            records.Add(New Tuple(Me, line))
            line = reader.ReadLine()
        Loop

        If expectedRecords <> numberOfRecords Then
            Throw New Exception("Mismatch of records to header record count")
        End If
    End Sub

    Public Overrides Function ToString() As String
        Return buildFriendlyOutput(numberOfAttributes, numberOfRecords, columns)
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

    Private Shared Function buildFriendlyOutput(numAttributes As Integer, numRecords As Integer, cols As ColumnList) As String
        Dim out As New StringBuilder
        out.AppendLine(String.Format("{0} {1}", numAttributes, Tools.pluralize("attribute", numAttributes)))
        For i As Integer = 0 To numAttributes - 1
            out.AppendLine(String.Format("Attribute {0}: {1}", i, cols(i)))
        Next
        out.Append(String.Format("{0} {1}", numRecords, Tools.pluralize("record", numRecords)))
        Return out.ToString
    End Function

    Public Shared Function readFromFile(name As String) As Table
        Dim contents As String = My.Computer.FileSystem.ReadAllText(name)
        Return New Table(contents)
    End Function

    Public Shared Function getHeaderFromFile(name As String) As String
        Dim stream As New IO.FileStream(name, IO.FileMode.Open)
        Try
            Dim reader As New IO.StreamReader(stream)
            Dim line As String
            Dim expectedAttributes As Integer
            Dim expectedRecords As Integer
            Dim headerLen As Integer = 0
            Dim cols As New ColumnList

            line = reader.ReadLine
            Dim matches As RegularExpressions.MatchCollection = RegularExpressions.Regex.Matches(line, "\[(?<val>[\w\d:]*)\]")
            If Not Integer.TryParse(matches(0).Groups("val").Value, expectedAttributes) Then
                Throw New Exception("Attribute count must be an integer")
            End If
            If Not Integer.TryParse(matches(matches.Count - 1).Groups("val").Value, expectedRecords) Then
                Throw New Exception("Record count must be an integer")
            End If
            headerLen += matches(0).Length + matches(matches.Count - 1).Length
            For i As Integer = 1 To matches.Count - 2
                cols.Add(New Column(matches(i).Value))
                headerLen += matches(i).Length
            Next
            If headerLen <> line.Length Then
                Throw New Exception("Junk in header")
            End If

            Return buildFriendlyOutput(expectedAttributes, expectedRecords, cols)
        Finally
            stream.Close()
        End Try
    End Function

    Public Sub saveToFile(name As String)
        Dim contents As String = ToString("f")
        My.Computer.FileSystem.WriteAllText(name, contents, False)
    End Sub

    Public Function search(condition As String) As TupleList
        Dim parts As String() = condition.Split(" ", options:=StringSplitOptions.RemoveEmptyEntries)

        If parts.Count <> 3 Then
            Throw New Exception("Expected 3 part condition")
        End If

        Dim col As Integer = -1
        Dim op As Tools.operators
        Dim val As Object

        For i As Integer = 0 To columns.Count - 1
            If parts(0).Equals(columns(i).name, StringComparison.CurrentCultureIgnoreCase) Then
                col = i
            End If
        Next
        If col = -1 Then
            Throw New Exception("Column does not exist")
        End If

        If parts(1) = ChrW(Tools.operators.equal).ToString Then
            op = operators.equal
        Else
            Throw New Exception("Invalid operator")
        End If

        val = columns(col).valToObj(parts(2))

        Return records.FindAll(Function(t As Tuple) t.values(col).Equals(val))
    End Function

End Class
