Module Day09
    Public Sub Day09_main()
        Dim testpath = "Day09\Day09_test01.txt"
        Dim inputpath = "Day09\Day09_input.txt"

        Dim testinput = Day09_ReadInput(testpath)
        Dim input = Day09_ReadInput(inputpath)

        Debug.Assert(Day09_Part01(testinput, 5) = 127, "Day09 Part1 test1 failed")
        Dim invalidnumber = Day09_Part01(input, 25)
        Console.WriteLine("Day09 Part1: " & invalidnumber)

        Debug.Assert(Day09_Part02(testinput, 127) = 62, "Day09 Part2 test1 failed")
        Console.WriteLine("Day09 Part2: " & Day09_Part02(input, invalidnumber))
    End Sub

    Public Function Day09_Part02(input As List(Of Int64), invalidnumber As Int64) As Int64
        Dim sumList As New List(Of Int64)

        sumList.Add(input(0))
        For i = 1 To input.Count - 1
            sumList.Add(input(i) + sumList(i - 1))
        Next

        For i = 0 To sumList.Count - 2
            Dim NumToFind = sumList(i) + invalidnumber
            For j = i To sumList.Count - 1
                If sumList(j) = NumToFind Then
                    'found solution, get max and min in range
                    Dim resultlist As New List(Of Int64)
                    For k = i + 1 To j
                        resultlist.Add(input(k))
                    Next
                    Return resultlist.Min + resultlist.Max
                    Exit Function
                End If
            Next
        Next

        Return -1
    End Function

    Public Function Day09_Part01(input As List(Of Int64), framesize As Integer) As Int64
        For i = framesize To input.Count - 1 'first frame cannot be checked
            Dim found As Boolean = False
            For j = i - framesize To i - 2
                Dim NumToSearch = input(i) - input(j)
                For k = j + 1 To i - 1
                    If input(k) = NumToSearch Then
                        found = True
                        Exit For
                    End If
                Next
                If found = True Then Exit For
            Next
            If found = False Then
                Return input(i)
                Exit Function
            End If
        Next
        Return 0
    End Function

    Public Function Day09_ReadInput(path As String) As List(Of Int64)
        Dim result As New List(Of Int64)

        Dim sr As New IO.StreamReader(path)
        While Not sr.EndOfStream
            result.Add(Convert.ToInt64(sr.ReadLine))
        End While

        Return result
    End Function

End Module
