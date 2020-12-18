Module Day18
    Public Sub Day18_main()
        Dim inputpath = "Day18\Day18_input.txt"

        Dim testinput1 = "1 + 2 * 3 + 4 * 5 + 6"
        Dim testinput2 = "1 + (2 * 3) + (4 * (5 + 6))"
        Dim testinput3 = "2 * 3 + (4 * 5)"
        Dim testinput4 = "5 + (8 * 3 + 9 + 3 * 4 * 3)"
        Dim testinput5 = "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))"
        Dim testinput6 = "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
        Dim input = Day18_ReadInput(inputpath)

        Debug.Assert(Day18_CalculateLine(testinput1, 1) = 71, "Day18 Part1 test1 failed")
        Debug.Assert(Day18_CalculateLine(testinput2, 1) = 51, "Day18 Part1 test2 failed")
        Debug.Assert(Day18_CalculateLine(testinput3, 1) = 26, "Day18 Part1 test3 failed")
        Debug.Assert(Day18_CalculateLine(testinput4, 1) = 437, "Day18 Part1 test4 failed")
        Debug.Assert(Day18_CalculateLine(testinput5, 1) = 12240, "Day18 Part1 test5 failed")
        Debug.Assert(Day18_CalculateLine(testinput6, 1) = 13632, "Day18 Part1 test6 failed")
        Console.WriteLine("Day18 Part1: " & Day18_Part01(input))

        Debug.Assert(Day18_CalculateLine(testinput1, 2) = 231, "Day18 Part2 test1 failed")
        Debug.Assert(Day18_CalculateLine(testinput2, 2) = 51, "Day18 Part2 test2 failed")
        Debug.Assert(Day18_CalculateLine(testinput3, 2) = 46, "Day18 Part2 test3 failed")
        Debug.Assert(Day18_CalculateLine(testinput4, 2) = 1445, "Day18 Part2 test4 failed")
        Debug.Assert(Day18_CalculateLine(testinput5, 2) = 669060, "Day18 Part2 test5 failed")
        Debug.Assert(Day18_CalculateLine(testinput6, 2) = 23340, "Day18 Part2 test6 failed")
        Console.WriteLine("Day18 Part2: " & Day18_Part02(input))
    End Sub

    Public Function Day18_Part02(input As List(Of String)) As UInt64
        Return input.Sum(Function(f) Day18_CalculateLine(f, 2))
    End Function

    Public Function Day18_Part01(input As List(Of String)) As UInt64
        Return input.Sum(Function(f) Day18_CalculateLine(f, 1))
    End Function
    Public Function Day18_CalculateLine(input As String, part As Integer) As UInt64
        While input.Contains("(")
            Dim openparant As Integer
            Dim closeparant As Integer
            For i = 0 To input.Count - 1
                If input(i) = "("c Then openparant = i
                If input(i) = ")"c Then
                    closeparant = i
                    Exit For
                End If
            Next
            Dim substring = input.Substring(openparant, closeparant - openparant + 1)
            Dim subresult = If(part = 1, Day18_CalculateSubResult(substring), Day18_CalculateSubResult2(substring))
            input = input.Replace(substring, subresult.ToString)
        End While
        Return If(part = 1, Day18_CalculateSubResult(input), Day18_CalculateSubResult2(input))
    End Function

    Public Function Day18_CalculateSubResult2(input As String) As UInt64
        input = input.Trim.TrimStart("(").TrimEnd(")").Trim()
        Dim attributes = input.Split(" ")

        For i = 1 To attributes.Count - 1
            If attributes(i) = "+" Then
                If attributes(i - 1).Contains(")") Then
                    attributes(i - 1) = attributes(i - 1).Replace(")", "")
                Else
                    attributes(i - 1) = "(" + attributes(i - 1)
                End If
                attributes(i + 1) = attributes(i + 1) + ")"
            End If
        Next

        Dim substring = String.Join(" ", attributes)
        Dim result = Day18_CalculateLine(substring, 1)
        Return result
    End Function

    Public Function Day18_CalculateSubResult(input As String) As UInt64
        input = input.Trim.TrimStart("(").TrimEnd(")").Trim()
        Dim attributes = input.Split(" ")
        Dim firstelement As UInt64 = 0
        Dim secondelement As UInt64 = 0
        Dim operation As String = ""

        firstelement = Convert.ToUInt64(attributes(0))
        For i = 1 To attributes.Count - 1
            Select Case attributes(i)
                Case "+"
                    operation = "+"
                Case "*"
                    operation = "*"
                Case Else
                    secondelement = Convert.ToUInt64(attributes(i))
                    If operation = "+" Then
                        firstelement = firstelement + secondelement
                    ElseIf operation = "*" Then
                        firstelement = firstelement * secondelement
                    Else
                        Throw New NotImplementedException
                    End If
            End Select
        Next
        Return firstelement
    End Function


    Public Function Day18_ReadInput(path As String) As List(Of String)
        Dim sr As New IO.StreamReader(path)
        Return sr.ReadToEnd.Split(vbCrLf).ToList
    End Function
End Module
