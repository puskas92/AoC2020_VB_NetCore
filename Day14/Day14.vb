Module Day14
    Public Sub Day14_main()
        Dim testpath = "Day14\Day14_test01.txt"
        Dim testpath2 = "Day14\Day14_test02.txt"
        Dim inputpath = "Day14\Day14_input.txt"

        Dim testinput = Day14_ReadInput(testpath)
        Dim testinput2 = Day14_ReadInput(testpath2)
        Dim input = Day14_ReadInput(inputpath)

        Debug.Assert(Day14_Part01(testinput) = 165, "Day14 Part1 test1 failed")
        Console.WriteLine("Day14 Part1: " & Day14_Part01(input))

        Debug.Assert(Day14_Part02(testinput2) = 208, "Day14 Part2 test1 failed")
        Console.WriteLine("Day14 Part2: " & Day14_Part02(input))
    End Sub

    Public Function Day14_Part02(input As List(Of Day14_Input)) As Int64
        Dim memory As New Dictionary(Of String, Int64)

        For Each inp In input
            Debug.Assert(inp.Mask.Length = 36)
            Dim add As String = ""
            For i = 35 To 0 Step -1
                If inp.Mask(35 - i) = "X"c Or inp.Mask(35 - i) = "1"c Then
                    add += inp.Mask(35 - i).ToString
                Else
                    add += If((inp.Memory.Item1 And Math.Pow(2, i)) > 0, "1"c, "0"c)
                End If
            Next

            'calculate overlapping
            If memory.ContainsKey(add) = True Then
                'trivial egzakt overlapping - should be overwritten
                memory(add) = inp.Memory.Item2
            Else

                'check partial overlapping
                Dim modifylist As New Dictionary(Of String, List(Of String))
                For Each key In memory.Keys
                    Dim overlap As Boolean = True
                    Dim newadd As String = ""
                    For i = 0 To 35
                        Select Case key(i)
                            Case "0"c
                                Select Case add(i)
                                    Case "0"c
                                        'overlap still true
                                        newadd += "0"c
                                    Case "1"c
                                        overlap = False
                                        Exit For
                                    Case "X"c
                                        'overlap still true
                                        newadd += "0"c  'total cover
                                End Select
                            Case "1"c
                                Select Case add(i)
                                    Case "0"c
                                        overlap = False
                                        Exit For
                                    Case "1"c
                                        'overlap still true
                                        newadd += "1"c
                                    Case "X"c
                                        'overlap still true
                                        newadd += "1"c 'total cover
                                End Select
                            Case "X"c
                                Select Case add(i)
                                    Case "0"c
                                        'overlap still true
                                        newadd += "E"c 'one or X
                                    Case "1"c
                                        'overlap still true
                                        newadd += "N"c 'null or X
                                    Case "X"c
                                        'overlap still true
                                        newadd += "X"c 'covered by full check
                                End Select
                            Case Else
                                Throw New Exception
                        End Select
                    Next
                    If overlap Then
                        modifylist.Add(key, New List(Of String)) 'handle full cover
                        For i = 0 To 35
                            If newadd(i) = "E"c Or newadd(i) = "N"c Then
                                Dim newadd2 = newadd.ToList
                                Dim newadd3 = newadd.ToList
                                newadd2(i) = If(newadd(i) = "E"c, "1"c, "0"c)

                                newadd3(i) = If(newadd(i) = "E"c, "0"c, "1"c)
                                newadd = String.Concat(newadd3)

                                Dim newaddstring = String.Concat(newadd2)
                                newaddstring = newaddstring.Replace("E", "X").Replace("N", "X")
                                modifylist(key).Add(newaddstring)
                            End If

                        Next

                    Else
                        Continue For
                    End If
                Next

                For Each elem In modifylist
                    Dim value = memory(elem.Key)
                    memory.Remove(elem.Key)
                    For Each addstring In elem.Value
                        If memory.ContainsKey(addstring) = True Then
                            memory(addstring) = value
                        Else
                            memory.Add(addstring, value)
                        End If
                    Next

                Next

                'memory itself should be written at the end of the list, since it is the newest one
                memory.Add(add, inp.Memory.Item2)
            End If
        Next

        'Return memory.Sum(Function(f) f.Value)
        Dim sum As Int64 = 0
        For Each mem In memory
            Dim numOfX = mem.Key.ToList.LongCount(Function(f) f = "X"c)
            sum += mem.Value * Math.Pow(2, numOfX)
        Next
        Return sum
    End Function

    Public Function Day14_Part01(input As List(Of Day14_Input)) As Int64
        Dim memory As New Dictionary(Of Integer, Int64)

        For Each inp In input
            Debug.Assert(inp.Mask.Length = 36)
            Dim value As Int64 = 0
            For i = 0 To 35
                If inp.Mask(35 - i) = "X"c Then
                    value += inp.Memory.Item2 And Math.Pow(2, i)
                Else
                    value += Convert.ToInt64(inp.Mask(35 - i).ToString) * Math.Pow(2, i)
                End If
            Next
            If memory.ContainsKey(inp.Memory.Item1) = False Then
                memory.Add(inp.Memory.Item1, value)
            Else
                memory(inp.Memory.Item1) = value
            End If
        Next

        Return memory.Sum(Function(f) f.Value)
    End Function


    Public Function Day14_ReadInput(path As String) As List(Of Day14_Input)
        Dim result As New List(Of Day14_Input)

        Dim sr As New IO.StreamReader(path)
        Dim mask As String = ""
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            If line.Contains("mask") Then
                mask = line.Split("=")(1).Trim
            Else
                Dim row As New Day14_Input
                row.Mask = mask
                Dim add As Int32 = Convert.ToInt32(line.Split("[")(1).Split("]")(0))
                Dim num As Int64 = Convert.ToInt64(line.Split("=")(1).Trim)
                Dim tup = New Tuple(Of Integer, Int64)(add, num)
                row.Memory = tup
                result.Add(row)
            End If
        End While

        Return result
    End Function

    Public Class Day14_Input
        Public Mask As String
        Public Memory As Tuple(Of Integer, Int64)
    End Class
End Module
