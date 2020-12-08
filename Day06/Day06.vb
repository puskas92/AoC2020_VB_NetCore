Module Day06
    Public Sub Day06_main()
        Dim testpath = "Day06\Day06_test01.txt"
        Dim inputpath = "Day06\Day06_input.txt"

        Dim testinput = Day06_ReadInput(testpath)
        Dim input = Day06_ReadInput(inputpath)

        Debug.Assert(Day06_Part01(testinput) = 11, "Day06 Part1 test1 failed")
        Console.WriteLine("Day06 Part1: " & Day06_Part01(input))

        Debug.Assert(Day06_Part02(testinput) = 6, "Day06 Part2 test1 failed")
        Console.WriteLine("Day06 Part2: " & Day06_Part02(input))
    End Sub

    Public Function Day06_ReadInput(path As String) As List(Of List(Of List(Of Char)))
        Dim result As New List(Of List(Of List(Of Char)))

        Dim sr As New IO.StreamReader(path)
        Dim group As New List(Of List(Of Char))
        While Not sr.EndOfStream
            Dim line = sr.ReadLine

            If line = "" Then
                result.Add(group)
                group = New List(Of List(Of Char))
                Continue While
            End If

            group.Add(line.ToCharArray.ToList)
        End While
        result.Add(group)

        Return result
    End Function

    Public Function Day06_Part01(input As List(Of List(Of List(Of Char))))
        Return input.Sum(Function(group)
                             Dim groupanswer As New List(Of Char)
                             For Each answer In group
                                 groupanswer = groupanswer.Union(answer).ToList
                             Next
                             Return groupanswer.Count
                         End Function)
    End Function

    Public Function Day06_Part02(input As List(Of List(Of List(Of Char))))
        Return input.Sum(Function(group)
                             Dim groupanswer As New List(Of Char)
                             groupanswer = group.First
                             For Each answer In group
                                 groupanswer = groupanswer.Intersect(answer).ToList
                             Next
                             Return groupanswer.Count
                         End Function)
    End Function
End Module
