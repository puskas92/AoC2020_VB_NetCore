Module Day10
    Public Sub Day10_main()
        Dim testpath1 = "Day10\Day10_test01.txt"
        Dim testpath2 = "Day10\Day10_test02.txt"
        Dim inputpath = "Day10\Day10_input.txt"

        'WARNING: input is preprocessed to differences
        Dim testinput1 = Day10_ReadInput(testpath1)
        Dim testinput2 = Day10_ReadInput(testpath2)
        Dim input = Day10_ReadInput(inputpath)

        Debug.Assert(Day10_Part01(testinput1) = 7 * 5, "Day10 Part1 test1 failed")
        Debug.Assert(Day10_Part01(testinput2) = 22 * 10, "Day10 Part1 test2 failed")
        Console.WriteLine("Day10 Part1: " & Day10_Part01(input))

        Debug.Assert(Day10_Part02(testinput1) = 8, "Day10 Part2 test1 failed")
        Debug.Assert(Day10_Part02(testinput2) = 19208, "Day10 Part2 test1 failed")
        Console.WriteLine("Day10 Part2: " & Day10_Part02(input))
    End Sub

    Public Function Day10_Part02(input As List(Of Integer)) As Int64
        Debug.Assert(input.LongCount(Function(f) f <> 1 AndAlso f <> 3) = 0, "Assertion for Part2 failed (diff values are only 1 and 3)")

        Dim distinct_ways As Int64 = 1

        Dim ConsequentOnes As New List(Of Integer)
        Dim consequent As Integer = 0
        For i = 0 To input.Count - 1
            If input(i) = 1 Then
                consequent += 1
            Else 'i=3
                If consequent <> 0 Then
                    ConsequentOnes.Add(consequent)
                    consequent = 0
                End If
            End If
        Next

        For Each consequent In ConsequentOnes
            distinct_ways *= DistinctWaysWithOnes(consequent)
        Next

        Return distinct_ways
    End Function

    Public Function Day10_Part01(input As List(Of Integer)) As Integer
        Dim One_count = input.LongCount(Function(f) f = 1)
        Dim Three_count = input.LongCount(Function(f) f = 3)

        Return One_count * Three_count
    End Function

    Public Function Day10_ReadInput(path As String) As List(Of Integer)
        Dim result As New List(Of Integer)

        Dim sr As New IO.StreamReader(path)
        While Not sr.EndOfStream
            result.Add(Convert.ToInt32(sr.ReadLine))
        End While

        result.Add(0)
        result.Add(result.Max + 3)
        result.Sort()

        Dim differenceList As New List(Of Integer)
        For i = 1 To result.Count - 1
            differenceList.Add(result(i) - result(i - 1))
        Next

        Return differenceList
    End Function

    Public DistinctWaysWithOnesCache As New Dictionary(Of Integer, Integer) 'From {{1, 1}, {2, 2}, {3, 4}, {4, 7}, {5, 13}}
    Public Function DistinctWaysWithOnes(consequentOnes As Integer) As Integer
        If DistinctWaysWithOnesCache.ContainsKey(consequentOnes) = False Then

            'create 034569 form base on consequentOnes
            Dim originInput As New List(Of Integer)
            originInput.Add(0)
            originInput.Add(3)
            For i = 0 To consequentOnes - 1
                originInput.Add(originInput(i + 1) + 1)
            Next
            originInput.Add(originInput.Last + 3)

            Dim result As Integer = 0

            For i As Integer = 0 To Math.Pow(2, consequentOnes) - 1
                Dim modifiedoriginalinput = New List(Of Integer)(originInput)

                Dim iInBinary As String = Convert.ToString(i, 2).PadLeft(consequentOnes, "0"c)

                For j = 0 To iInBinary.Count - 1
                    If iInBinary(j) = "1"c Then
                        'modifiedoriginalinput.RemoveAt(j + 2)
                        modifiedoriginalinput(j + 2) = -1 'avoid changing length
                    End If
                Next
                modifiedoriginalinput.RemoveAll(Function(f) f = -1)

                'check if valid
                Dim newdiff As New List(Of Integer)
                For j = 1 To modifiedoriginalinput.Count - 1
                    newdiff.Add(modifiedoriginalinput(j) - modifiedoriginalinput(j - 1))
                Next

                If newdiff.LongCount(Function(f) f > 3) = 0 Then result += 1
            Next

            DistinctWaysWithOnesCache.Add(consequentOnes, result)
        End If
        Return DistinctWaysWithOnesCache(consequentOnes)
    End Function

    Public DistinctWaysWithOneTwoThreeCache As New Dictionary(Of String, Integer)
    Public Function DistinctWaysWithOneTwoThree(input As List(Of Integer)) As Integer
        'Debug.Assert(input.Count >= 3, "error")
        If input.Count <= 3 Then Return 1

        Dim inputasstring As String = String.Concat(input.ConvertAll(Function(f) f.ToString))

        If DistinctWaysWithOneTwoThreeCache.ContainsKey(inputasstring) = False Then
            Dim result As Integer = 1
            Dim originInput As New List(Of Integer)
            originInput.Add(0)
            For i = 0 To input.Count - 1
                originInput.Add(originInput(i) + input(i))
            Next
            For i = 1 To input.Count - 2
                Dim modifiedoriginalinput = New List(Of Integer)(originInput)
                modifiedoriginalinput.RemoveAt(i)


                Dim newdiff As New List(Of Integer)
                For j = 1 To modifiedoriginalinput.Count - 1
                    newdiff.Add(modifiedoriginalinput(j) - modifiedoriginalinput(j - 1))
                Next

                If newdiff(1) = 3 Then
                    newdiff.RemoveAt(0)
                End If
                If newdiff(newdiff.Count - 2) = 3 Then
                    newdiff.RemoveAt(newdiff.Count - 1)
                End If

                If newdiff.LongCount(Function(f) f > 3) = 0 Then
                    result += DistinctWaysWithOneTwoThree(newdiff)
                End If
            Next

            DistinctWaysWithOneTwoThreeCache.Add(inputasstring, result)
        End If
        Return DistinctWaysWithOneTwoThreeCache(inputasstring)
    End Function
End Module
