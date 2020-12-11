Module Day11
    Public Sub Day11_main()
        Dim testpath = "Day11\Day11_test01.txt"
        Dim inputpath = "Day11\Day11_input.txt"

        Dim testinput = Day11_ReadInput(testpath)
        Dim input = Day11_ReadInput(inputpath)

        Debug.Assert(Day11(testinput, 1) = 37, "Day11 Part1 test1 failed")
        Console.WriteLine("Day11 Part1: " & Day11(input, 1))

        testinput = Day11_ReadInput(testpath)
        input = Day11_ReadInput(inputpath)
        Debug.Assert(Day11(testinput, 2) = 26, "Day11 Part2 test1 failed")
        Console.WriteLine("Day11 Part2: " & Day11(input, 2))
    End Sub

    Public Function Day11(input As List(Of List(Of Day11_GridElement)), part As Integer) As Integer
        Dim Changed As Boolean = False

        Do
            Changed = CalculateNextValue(input, part)
            'DisplayDay11Input(input)
        Loop While Changed

        Return input.Sum(Function(f) f.LongCount(Function(g) g.CurrentValue = "#"c))
    End Function

    Private Sub DisplayDay11Input(input As List(Of List(Of Day11_GridElement)))
        Console.WriteLine()
        For Each line In input
            Console.WriteLine(String.Concat(line.ConvertAll(Function(f) f.CurrentValue)))
        Next
    End Sub

    Public Function CalculateNextValue(input As List(Of List(Of Day11_GridElement)), type As Integer) As Boolean
        Dim changed As Boolean = False
        For i = 1 To input.Count - 2
            For j = 1 To input(i).Count - 2
                Dim NoOfAdjacentOccupied As Integer
                If type = 1 Then
                    NoOfAdjacentOccupied = Day11_Part1OccupiedCount(input, i, j)
                Else
                    NoOfAdjacentOccupied = Day11_Part2OccupiedCount(input, i, j)
                End If

                Select Case input(i)(j).CurrentValue
                    Case "#"c
                        If NoOfAdjacentOccupied >= If(type = 1, 4, 5) Then
                            input(i)(j).NextValue = "L"c
                            changed = True
                        End If
                    Case "L"c
                        If NoOfAdjacentOccupied = 0 Then
                            input(i)(j).NextValue = "#"c
                            changed = True
                        End If
                    Case Else
                        input(i)(j).NextValue = "."c
                End Select
            Next
        Next

        input.ForEach(Sub(f) f.ForEach(Sub(g) g.ChangeValue()))

        Return changed
    End Function

    Public Function Day11_Part1OccupiedCount(input As List(Of List(Of Day11_GridElement)), i As Integer, j As Integer) As Integer
        Dim NoOfAdjacentOccupied As Integer = 0
        For k = -1 To 1
            For l = -1 To 1
                If k = 0 AndAlso l = 0 Then Continue For

                If input(i + k)(j + l).CurrentValue = "#"c Then NoOfAdjacentOccupied += 1

            Next
        Next

        Return NoOfAdjacentOccupied
    End Function

    Public Function Day11_Part2OccupiedCount(input As List(Of List(Of Day11_GridElement)), i As Integer, j As Integer) As Integer
        Dim NoOfAdjacentOccupied As Integer = 0
        Dim found As Boolean = False
        Dim step1 As Integer = 1
        For k = -1 To 1
            For l = -1 To 1
                If k = 0 AndAlso l = 0 Then Continue For
                found = False
                step1 = 1
                Do
                    Dim x = i + k * step1
                    Dim y = j + l * step1

                    If x > 0 AndAlso x < input.Count - 1 AndAlso y > 0 AndAlso y < input(x).Count - 1 Then
                        Select Case input(x)(y).CurrentValue
                            Case "."c
                                'continue
                            Case "L"c
                                found = True
                                'no increase
                            Case "#"c
                                found = True
                                NoOfAdjacentOccupied += 1
                        End Select
                    Else
                        'went out of grid
                        found = True
                    End If

                    step1 += 1
                Loop Until found
            Next
        Next

        Return NoOfAdjacentOccupied
    End Function

    Public Function Day11_ReadInput(path As String) As List(Of List(Of Day11_GridElement))
        Dim result As New List(Of List(Of Day11_GridElement))

        Dim sr As New IO.StreamReader(path)
        Dim first As Boolean = True
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            Dim lineAsGrid As New List(Of Day11_GridElement)
            lineAsGrid.Add(New Day11_GridElement("."))
            lineAsGrid.AddRange(line.ToList.ConvertAll(Function(f) New Day11_GridElement(f)))
            lineAsGrid.Add(New Day11_GridElement("."))

            If first Then
                Dim dummyline1 As New List(Of Day11_GridElement)
                For Each c In lineAsGrid
                    dummyline1.Add(New Day11_GridElement("."))
                Next
                result.Add(dummyline1)
                first = False
            End If

            result.Add(lineAsGrid)
        End While

        Dim dummyline2 As New List(Of Day11_GridElement)
        For Each c In result(0)
            dummyline2.Add(New Day11_GridElement("."))
        Next
        result.Add(dummyline2)
        first = False

        Return result
    End Function

    Public Class Day11_GridElement
        Public CurrentValue As Char
        Public NextValue As Char

        Public Sub New(startvalue As Char)
            CurrentValue = startvalue
        End Sub

        Public Sub ChangeValue()
            CurrentValue = NextValue
        End Sub
    End Class
End Module
