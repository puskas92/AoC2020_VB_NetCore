Module Day08
    Public Sub Day08_main()
        Dim testpath = "Day08\Day08_test01.txt"
        Dim inputpath = "Day08\Day08_input.txt"

        Dim testinput = Day08_ReadInput(testpath)
        Dim input = Day08_ReadInput(inputpath)

        Debug.Assert(Day08_Part01(testinput) = 5, "Day08 Part1 test1 failed")
        Console.WriteLine("Day08 Part1: " & Day08_Part01(input))

        Debug.Assert(Day08_Part02(testinput) = 8, "Day08 Part2 test1 failed")
        Console.WriteLine("Day08 Part2: " & Day08_Part02(input))
    End Sub

    Public Function Day08_Part01(input As List(Of KeyValuePair(Of String, Integer))) As Integer
        Dim result = Day08_RunCode(input)
        Return result.Item2
    End Function

    Public Function Day08_Part02(input As List(Of KeyValuePair(Of String, Integer))) As Integer
        Dim result As Integer = 0
        For i = 0 To input.Count - 1
            If input(i).Key = "acc" Then Continue For

            Dim modifiedinput As New List(Of KeyValuePair(Of String, Integer))(input)
            If input(i).Key = "jmp" Then
                modifiedinput(i) = New KeyValuePair(Of String, Integer)("nop", input(i).Value)
            ElseIf input(i).Key = "nop" Then
                modifiedinput(i) = New KeyValuePair(Of String, Integer)("jmp", input(i).Value)
            Else
                Throw New NotImplementedException
            End If

            Dim subresult = Day08_RunCode(modifiedinput)
            If subresult.Item1 = True Then
                result = subresult.Item2
                Exit For
            End If
        Next
        Return result
    End Function

    Private Function Day08_RunCode(input As List(Of KeyValuePair(Of String, Integer))) As (Boolean, Integer)
        Dim acc As Integer = 0
        Dim program As Integer = 0
        Dim terminateprogram As Integer = input.Count
        Dim isTerminated As Boolean = False

        Dim program_cache As New List(Of Integer)

        While program_cache.Contains(program) = False
            program_cache.Add(program)
            Dim line = input(program)
            Select Case line.Key
                Case "nop"
                    'no operation
                    program += 1
                Case "acc"
                    acc += line.Value
                    program += 1
                Case "jmp"
                    program += line.Value
                Case Else
                    Throw New NotImplementedException
            End Select
            If program = terminateprogram Then
                isTerminated = True
                Exit While
            End If
        End While

        Return (isTerminated, acc)
    End Function

    Public Function Day08_ReadInput(path As String) As List(Of KeyValuePair(Of String, Integer))
        Dim result As New List(Of KeyValuePair(Of String, Integer))

        Dim sr As New IO.StreamReader(path)
        While Not sr.EndOfStream
            'nop +0
            Dim line = sr.ReadLine
            Dim Key = line.Split(" ")(0)
            Dim Value = Convert.ToInt32(line.Split(" ")(1))

            result.Add(New KeyValuePair(Of String, Integer)(Key, Value))
        End While

        Return result
    End Function

End Module
