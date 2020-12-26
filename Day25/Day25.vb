Module Day25
    Public Sub Day25_main()
        Dim testpath = "Day25\Day25_test01.txt"
        Dim inputpath = "Day25\Day25_input.txt"

        Dim testinput = Day25_ReadInput(testpath)
        Dim input = Day25_ReadInput(inputpath)

        Debug.Assert(Day25_Part01(testinput) = 14897079, "Day25 Part1 test1 failed")
        Console.WriteLine("Day25 Part1: " & Day25_Part01(input))
    End Sub

    Public Function Day25_Part01(input As Day25_Input) As Integer
        Dim value As Int64 = 1
        Dim subject_number = 7

        Dim loopnumber As Integer = 0
        While value <> input.CardPublicKey
            loopnumber += 1
            value *= subject_number
            value = value Mod 20201227
        End While

        value = 1
        For i = 0 To loopnumber - 1
            value *= input.DoorPublicKey
            value = value Mod 20201227
        Next

        Return value
    End Function


    Public Function Day25_ReadInput(path As String) As Day25_Input
        Dim result As New Day25_Input

        Dim sr As New IO.StreamReader(path)
        result.CardPublicKey = Convert.ToInt32(sr.ReadLine)
        result.DoorPublicKey = Convert.ToInt32(sr.ReadLine)

        Return result
    End Function


    Public Class Day25_Input
        Public DoorPublicKey
        Public CardPublicKey
    End Class
End Module
