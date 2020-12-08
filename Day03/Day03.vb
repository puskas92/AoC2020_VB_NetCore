Module Day03
    Public Sub Day03_main()
        Dim testpath = "Day03\Day03_test01.txt"
        Dim inputpath = "Day03\Day03_input.txt"

        Dim testmap As Dictionary(Of Integer, Dictionary(Of Integer, Boolean)) = Day03_ReadInput(testpath)
        Dim inputmap As Dictionary(Of Integer, Dictionary(Of Integer, Boolean)) = Day03_ReadInput(inputpath)

        Debug.Assert(Day03_Part1(testmap) = 7, "Day03 Part1 test failed")
        Console.WriteLine("Day03 Part1: " & Day03_Part1(inputmap))


        Debug.Assert(Day03_Part2(testmap) = 336, "Day03 Part2 test failed")
        Debug.Assert(Day03_SlideDown(testmap, 1, 1) = 2, "Day03 Part2 Case 1,1")
        Debug.Assert(Day03_SlideDown(testmap, 3, 1) = 7, "Day03 Part2 Case 3,1")
        Debug.Assert(Day03_SlideDown(testmap, 5, 1) = 3, "Day03 Part2 Case 5,1")
        Debug.Assert(Day03_SlideDown(testmap, 7, 1) = 4, "Day03 Part2 Case 7,1")
        Debug.Assert(Day03_SlideDown(testmap, 1, 2) = 2, "Day03 Part2 Case 1,2")
        Console.WriteLine("Day03 Part2: " & Day03_Part2(inputmap))
    End Sub

    Public Function Day03_Part1(map As Dictionary(Of Integer, Dictionary(Of Integer, Boolean))) As Integer
        'right: 3 down: 1
        Dim right = 3
        Dim down = 1

        Return Day03_SlideDown(map, right, down)
    End Function

    Public Function Day03_Part2(map As Dictionary(Of Integer, Dictionary(Of Integer, Boolean))) As Int64
        Dim result As Int64 = 1
        Dim subresult As Integer
        'right 1, down 1.
        subresult = Day03_SlideDown(map, 1, 1)
        'Debug.Assert(subresult = 2, "Day03 Part2 Case 1,1")
        result = result * subresult

        'right: 3 down: 1
        subresult = Day03_SlideDown(map, 3, 1)
        'Debug.Assert(subresult = 7, "Day03 Part2 Case 3,1")
        result = result * subresult

        'Right 5, down 1
        subresult = Day03_SlideDown(map, 5, 1)
        'Debug.Assert(subresult = 3, "Day03 Part2 Case 5,1")
        result = result * subresult

        'Right 7, down 1.
        subresult = Day03_SlideDown(map, 7, 1)
        result = result * subresult

        'Right 1, down 2.
        subresult = Day03_SlideDown(map, 1, 2)
        result = result * subresult

        Return result

    End Function

    Private Function Day03_SlideDown(map As Dictionary(Of Integer, Dictionary(Of Integer, Boolean)), right As Integer, down As Integer) As Integer
        Dim maxX = map(0).Keys.Max
        Dim maxY = map.Keys.Max

        Dim hitcount As Integer = 0
        For i = 0 To maxY Step down
            If map(i)(((i / down) * right) Mod (maxX + 1)) = True Then hitcount = hitcount + 1
        Next

        Return hitcount
    End Function

    Private Function Day03_ReadInput(path As String) As Dictionary(Of Integer, Dictionary(Of Integer, Boolean))
        Dim map As New Dictionary(Of Integer, Dictionary(Of Integer, Boolean))

        Dim sr As New IO.StreamReader(path)
        Dim x, y As Integer
        y = -1
        While (Not (sr.EndOfStream))
            Dim line = sr.ReadLine
            y = y + 1
            map.Add(y, New Dictionary(Of Integer, Boolean))
            x = -1
            For Each c As Char In line
                x = x + 1
                map(y).Add(x, If(c = ".", False, True))
            Next
        End While

        Return map
    End Function
End Module