Module Day15
    Public Sub Day15_main()
        Dim input() As Integer = {12, 20, 0, 6, 1, 17, 7}

        Debug.Assert(Day15_Part01({0, 3, 6}, 2020) = 436, "Day15 Part1 test1 failed")
        Debug.Assert(Day15_Part01({1, 3, 2}, 2020) = 1, "Day15 Part1 test2 failed")
        Debug.Assert(Day15_Part01({2, 1, 3}, 2020) = 10, "Day15 Part1 test3 failed")
        Debug.Assert(Day15_Part01({1, 2, 3}, 2020) = 27, "Day15 Part1 test4 failed")
        Debug.Assert(Day15_Part01({2, 3, 1}, 2020) = 78, "Day15 Part1 test5 failed")
        Debug.Assert(Day15_Part01({3, 2, 1}, 2020) = 438, "Day15 Part1 test6 failed")
        Debug.Assert(Day15_Part01({3, 1, 2}, 2020) = 1836, "Day15 Part1 test7 failed")
        Console.WriteLine("Day15 Part1: " & Day15_Part01(input, 2020))

        Debug.Assert(Day15_Part02({0, 3, 6}, 30000000) = 175594, "Day15 Part2 test1 failed")
        Debug.Assert(Day15_Part02({1, 3, 2}, 30000000) = 2578, "Day15 Part2 test2 failed")
        Debug.Assert(Day15_Part02({2, 1, 3}, 30000000) = 3544142, "Day15 Part2 test3 failed")
        Debug.Assert(Day15_Part02({1, 2, 3}, 30000000) = 261214, "Day15 Part2 test4 failed")
        Debug.Assert(Day15_Part02({2, 3, 1}, 30000000) = 6895259, "Day15 Part2 test5 failed")
        Debug.Assert(Day15_Part02({3, 2, 1}, 30000000) = 18, "Day15 Part2 test6 failed")
        Debug.Assert(Day15_Part02({3, 1, 2}, 30000000) = 362, "Day15 Part2 test7 failed")
        Console.WriteLine("Day15 Part2: " & Day15_Part02(input, 30000000))
    End Sub


    Public Function Day15_Part01(input() As Integer, iter As Integer) As Integer
        Dim game As New List(Of Integer)
        game.AddRange(input)

        For i = game.Count To iter - 1
            If game.LongCount(Function(f) f = game.Last) = 1 Then
                game.Add(0)
            Else
                For j = i - 2 To 0 Step -1
                    If game(j) = game(i - 1) Then
                        game.Add((i - 1) - j)
                        Exit For
                    End If
                Next
            End If
        Next

        Return game.Last
    End Function

    Public Function Day15_Part02(input() As Integer, iter As Integer) As Integer
        Dim game As New Dictionary(Of Integer, Integer)
        For i = 1 To input.Count
            'assert that in the input there is no number appears twice
            game.Add(input(i - 1), i)
        Next
        Dim lastnumber = input.Last
        Dim lastnumberplace As Integer = -1

        For i = input.Count + 1 To iter
            If lastnumberplace = -1 Then
                lastnumber = 0

            Else
                lastnumber = (i - 1) - lastnumberplace
            End If

            If game.ContainsKey(lastnumber) Then
                lastnumberplace = game(lastnumber)
                game(lastnumber) = i
            Else
                lastnumberplace = -1
                game.Add(lastnumber, i)
            End If
        Next

        Return lastnumber
    End Function
End Module
