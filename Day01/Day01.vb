Module Day01
    Sub Day01_main()
        'part1
        Debug.Assert(Day01_Part1("Day01\Day01_test01.txt") = 514579)
        Console.WriteLine("Day01 Part1: " & Day01_Part1("Day01\Day01_input.txt"))

        'part2
        Debug.Assert(Day01_Part2("Day01\Day01_test01.txt") = 241861950)
        Console.WriteLine("Day01 Part2: " & Day01_Part2("Day01\Day01_input.txt"))

    End Sub

    Function Day01_Part1(inputpath As String) As Integer
        Dim input As New List(Of Integer)

        Dim sr As New IO.StreamReader(inputpath)
        While (Not sr.EndOfStream)
            input.Add(Convert.ToInt32(sr.ReadLine))
        End While

        Dim i, j As Integer
        Dim found As Boolean = False
        For i = 0 To input.Count - 2
            For j = i + 1 To input.Count - 1
                If (input(i) + input(j) = 2020) Then found = True
                If found Then Exit For
            Next
            If found Then Exit For
        Next

        Return (input(i) * input(j))
    End Function

    Function Day01_Part2(inputpath As String) As Integer
        Dim input As New List(Of Integer)

        Dim sr As New IO.StreamReader(inputpath)
        While (Not sr.EndOfStream)
            input.Add(Convert.ToInt32(sr.ReadLine))
        End While

        Dim i, j, k As Integer
        Dim found As Boolean = False
        For i = 0 To input.Count - 3
            For j = i + 1 To input.Count - 2
                If (input(i) + input(j) > 2020) Then Continue For

                For k = j + 1 To input.Count - 1
                    If (input(i) + input(j) + input(k) = 2020) Then found = True
                    If found Then Exit For
                Next
                If found Then Exit For
            Next
            If found Then Exit For
        Next

        Return (input(i) * input(j) * input(k))
    End Function
End Module
