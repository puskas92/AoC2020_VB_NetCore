Imports System.Numerics

Module Day13
    Public Sub Day13_main()
        Dim testpath = "Day13\Day13_test01.txt"
        Dim inputpath = "Day13\Day13_input.txt"

        Dim testinput = Day13_ReadInput(testpath)
        Dim input = Day13_ReadInput(inputpath)

        Debug.Assert(Day13_Part01(testinput) = 295, "Day13 Part1 test1 failed")
        Console.WriteLine("Day13 Part1: " & Day13_Part01(input))

        Debug.Assert(Day13_Part02(testinput.Buses) = 1068781, "Day13 Part2 test1 failed")
        Debug.Assert(Day13_Part02({17, -1, 13, 19}.ToList) = 3417, "Day13 Part2 test2 failed")
        Debug.Assert(Day13_Part02({67, 7, 59, 61}.ToList) = 754018, "Day13 Part2 test3 failed")
        Debug.Assert(Day13_Part02({67, -1, 7, 59, 61}.ToList) = 779210, "Day13 Part2 test4 failed")
        Debug.Assert(Day13_Part02({67, 7, -1, 59, 61}.ToList) = 1261476, "Day13 Part2 test5 failed")
        Debug.Assert(Day13_Part02({1789, 37, 47, 1889}.ToList) = 1202161486, "Day13 Part2 test6 failed")
        Console.WriteLine("Day13 Part2: " & Day13_Part02(input.Buses))
    End Sub

    Public Function Day13_Part02(input As List(Of Integer)) As UInt64
        Dim inputdict As New Dictionary(Of Int64, Int64) 'place, value
        For i = 0 To input.Count - 1
            If input(i) <> -1 Then
                inputdict.Add(i, input(i))
            End If
        Next

        Dim sum As Int64 = 0
        'need to summarize the multipliplation of other elements, the remainder (-1*place) and the modinv of multiofothers and the number itself
        For Each inp In inputdict
            Dim MultiOfOthers As UInt64 = 1
            For Each inp2 In inputdict
                If inp.Key = inp2.Key Then Continue For
                MultiOfOthers *= inp2.Value
            Next
            Dim ModinvWithOthers As UInt64 = modinv(MultiOfOthers, inp.Value)

            sum += (-1 * inp.Key * ModinvWithOthers * MultiOfOthers)
        Next

        'the sum is valid number, but not the smallest, it must be divided by the LCM of inputs

        Dim LCM As Int64 = MathNet.Numerics.Euclid.LeastCommonMultiple(inputdict.Values.ToList)
        sum = Mod2(sum, LCM)
        Return sum
    End Function

    Public Function Mod2(x As Int64, m As Int64) As Int64
        Return ((x Mod m) + m) Mod m
    End Function

    Public Function modinv(a As BigInteger, b As BigInteger) As BigInteger
        'works only when b is prime
        Return BigInteger.ModPow(a, (b - 2), b)
    End Function

    Public Function Day13_Part01(input As Day13_input) As Integer
        Dim BusCatched As Integer = -1
        Dim time As Integer = -1
        Do
            time += 1
            Dim currenttime = input.StartTime + time
            For Each bus In input.Buses
                If bus = -1 Then Continue For
                If currenttime Mod bus = 0 Then
                    BusCatched = bus
                    Exit For
                End If
            Next
        Loop While BusCatched = -1

        Return time * BusCatched
    End Function


    Public Function Day13_ReadInput(path As String) As Day13_input
        Dim result As New Day13_input

        Dim sr As New IO.StreamReader(path)
        result.StartTime = Convert.ToInt32(sr.ReadLine)
        Dim line = sr.ReadLine
        result.Buses = line.Split(",").ToList.ConvertAll(Function(f) If(f <> "x", Convert.ToInt32(f), -1))
        Return result
    End Function
    Public Class Day13_input
        Public StartTime As Integer
        Public Buses As New List(Of Integer) '-1=X
    End Class
End Module
