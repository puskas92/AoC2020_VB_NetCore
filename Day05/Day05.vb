Module Day05
    Public Sub Day05_main()
        Dim inputpath = "Day05\Day05_input.txt"

        Debug.Assert(New Day05_Seat("BFFFBBFRRR").seatId = 567, "Day5 Part1 Test1")
        Debug.Assert(New Day05_Seat("FFFBBBFRRR").seatId = 119, "Day5 Part1 Test2")
        Debug.Assert(New Day05_Seat("BBFFBBFRLL").seatId = 820, "Day5 Part1 Test3")

        Dim tickets = Day05_ReadInput(inputpath)
        Console.WriteLine("Day05 Part1: " & tickets.Max(Function(f) f.seatId))

        Console.WriteLine("Day05 Part2: " & Day05_Part2(tickets))

    End Sub

    Public Function Day05_Part2(tickets As List(Of Day05_Seat)) As Integer
        Dim min = tickets.Min(Function(f) f.seatId)
        Dim max = tickets.Max(Function(f) f.seatId)

        Dim i As Integer
        For i = min To max
            If tickets.Exists(Function(f) f.seatId = i) = False Then Exit For
        Next
        Return i
    End Function


    Public Function Day05_ReadInput(path As String) As List(Of Day05_Seat)
        Dim result As New List(Of Day05_Seat)

        Dim sr As New IO.StreamReader(path)
        While Not sr.EndOfStream
            result.Add(New Day05_Seat(sr.ReadLine))
        End While

        Return result
    End Function

    Public Class Day05_Seat
        Public rawtext As String
        Public binarystring As String
        Public row As Integer
        Public column As Integer
        Public seatId As Integer

        Public Sub New(rawtext As String)
            Me.rawtext = rawtext
            binarystring = rawtext.Replace("F", "0").Replace("B", "1").Replace("R", "1").Replace("L", "0")
            seatId = Convert.ToInt32(binarystring, 2)
            row = Convert.ToInt32(Left(binarystring, 7), 2)
            column = Convert.ToInt32(Right(binarystring, 3), 2)
        End Sub
    End Class
End Module
