Module Day17
    Public Sub Day17_main()
        Dim testpath = "Day17\Day17_test01.txt"
        Dim inputpath = "Day17\Day17_input.txt"

        Dim testinput = Day17_ReadInput(testpath)
        Dim input = Day17_ReadInput(inputpath)

        'Day17_DisplayGrid(testinput, 0)
        Debug.Assert(Day17_Part01(testinput, 6) = 112, "Day17 Part1 test1 failed")
        Console.WriteLine("Day17 Part1: " & Day17_Part01(input, 6))


        Dim testinput2 = Day17_ReadInput2(testpath)
        Dim input2 = Day17_ReadInput2(inputpath)
        Debug.Assert(Day17_Part02(testinput2, 6) = 848, "Day17 Part2 test1 failed")
        Console.WriteLine("Day17 Part2: " & Day17_Part02(input2, 6))
    End Sub

    Public Function Day17_Part01(grid As Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point))), cycle As Integer) As Integer
        'Dim directions As New List(Of Day17_Point3D)({New Day17_Point3D(1, 0, 0), New Day17_Point3D(-1, 0, 0), New Day17_Point3D(0, 1, 0), New Day17_Point3D(0, -1, 0), New Day17_Point3D(0, 0, 1), New Day17_Point3D(0, 0, -1)})

        For time = 1 To cycle
            'calculate active neighbours
            For x = grid.Keys.Min To grid.Keys.Max
                If grid.ContainsKey(x) = False Then Continue For
                For y = grid(x).Keys.Min To grid(x).Keys.Max
                    If grid(x).ContainsKey(y) = False Then Continue For
                    For z = grid(x)(y).Keys.Min To grid(x)(y).Keys.Max
                        If grid(x)(y).ContainsKey(z) = False Then Continue For
                        If grid(x)(y)(z).CurrentState = False Then Continue For

                        For i = -1 To 1
                            For j = -1 To 1
                                For k = -1 To 1
                                    If i = 0 AndAlso j = 0 AndAlso k = 0 Then Continue For
                                    If grid.ContainsKey(x + i) = False Then grid.Add(x + i, New Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point)))
                                    If grid(x + i).ContainsKey(y + j) = False Then grid(x + i).Add(y + j, New Dictionary(Of Integer, Day17_Grid_Point))
                                    If grid(x + i)(y + j).ContainsKey(z + k) = False Then grid(x + i)(y + j).Add(z + k, New Day17_Grid_Point)

                                    grid(x + i)(y + j)(z + k).ActiveNeighbours += 1
                                Next
                            Next
                        Next

                    Next
                Next
            Next

            'calculate next state
            For x = grid.Keys.Min - 1 To grid.Keys.Max + 1
                If grid.ContainsKey(x) = False Then Continue For
                For y = grid(x).Keys.Min To grid(x).Keys.Max
                    If grid(x).ContainsKey(y) = False Then Continue For
                    For z = grid(x)(y).Keys.Min To grid(x)(y).Keys.Max
                        If grid(x)(y).ContainsKey(z) = False Then Continue For

                        Dim actCell = grid(x)(y)(z)
                        If actCell.CurrentState = True Then
                            If actCell.ActiveNeighbours = 2 OrElse actCell.ActiveNeighbours = 3 Then
                                grid(x)(y)(z).NextState = True
                            Else
                                grid(x)(y)(z).NextState = False
                            End If
                        Else
                            If actCell.ActiveNeighbours = 3 Then
                                grid(x)(y)(z).NextState = True
                            Else
                                grid(x)(y)(z).NextState = False
                            End If
                        End If
                    Next
                Next
            Next

            'update state
            For Each x In grid
                For Each y In x.Value
                    For Each z In y.Value
                        z.Value.ChangeState()
                    Next
                Next
            Next

            'Day17_DisplayGrid(grid, time)
        Next
        Return grid.Sum(Function(f) f.Value.Sum(Function(g) g.Value.LongCount(Function(h) h.Value.CurrentState)))
    End Function

    Public Function Day17_Part02(grid As Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point)))), cycle As Integer) As Integer
        'Dim directions As New List(Of Day17_Point3D)({New Day17_Point3D(1, 0, 0), New Day17_Point3D(-1, 0, 0), New Day17_Point3D(0, 1, 0), New Day17_Point3D(0, -1, 0), New Day17_Point3D(0, 0, 1), New Day17_Point3D(0, 0, -1)})

        For time = 1 To cycle
            'calculate active neighbours
            For x = grid.Keys.Min To grid.Keys.Max
                If grid.ContainsKey(x) = False Then Continue For
                For y = grid(x).Keys.Min To grid(x).Keys.Max
                    If grid(x).ContainsKey(y) = False Then Continue For
                    For z = grid(x)(y).Keys.Min To grid(x)(y).Keys.Max
                        If grid(x)(y).ContainsKey(z) = False Then Continue For
                        For w = grid(x)(y)(z).Keys.Min To grid(x)(y)(z).Keys.Max
                            If grid(x)(y)(z).ContainsKey(w) = False Then Continue For
                            If grid(x)(y)(z)(w).CurrentState = False Then Continue For

                            For i = -1 To 1
                                For j = -1 To 1
                                    For k = -1 To 1
                                        For l = -1 To 1
                                            If i = 0 AndAlso j = 0 AndAlso k = 0 AndAlso l = 0 Then Continue For
                                            If grid.ContainsKey(x + i) = False Then grid.Add(x + i, New Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point))))
                                            If grid(x + i).ContainsKey(y + j) = False Then grid(x + i).Add(y + j, New Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point)))
                                            If grid(x + i)(y + j).ContainsKey(z + k) = False Then grid(x + i)(y + j).Add(z + k, New Dictionary(Of Integer, Day17_Grid_Point))
                                            If grid(x + i)(y + j)(z + k).ContainsKey(w + l) = False Then grid(x + i)(y + j)(z + k).Add(w + l, New Day17_Grid_Point)

                                            grid(x + i)(y + j)(z + k)(w + l).ActiveNeighbours += 1
                                        Next
                                    Next
                                Next
                            Next
                        Next
                    Next
                Next
            Next

            'calculate next state
            For x = grid.Keys.Min - 1 To grid.Keys.Max + 1
                If grid.ContainsKey(x) = False Then Continue For
                For y = grid(x).Keys.Min To grid(x).Keys.Max
                    If grid(x).ContainsKey(y) = False Then Continue For
                    For z = grid(x)(y).Keys.Min To grid(x)(y).Keys.Max
                        If grid(x)(y).ContainsKey(z) = False Then Continue For
                        For w = grid(x)(y)(z).Keys.Min To grid(x)(y)(z).Keys.Max
                            If grid(x)(y)(z).ContainsKey(w) = False Then Continue For

                            Dim actCell = grid(x)(y)(z)(w)
                            If actCell.CurrentState = True Then
                                If actCell.ActiveNeighbours = 2 OrElse actCell.ActiveNeighbours = 3 Then
                                    actCell.NextState = True
                                Else
                                    actCell.NextState = False
                                End If
                            Else
                                If actCell.ActiveNeighbours = 3 Then
                                    actCell.NextState = True
                                Else
                                    actCell.NextState = False
                                End If
                            End If
                        Next
                    Next
                Next
            Next

            'update state
            For Each x In grid
                For Each y In x.Value
                    For Each z In y.Value
                        For Each w In z.Value
                            w.Value.ChangeState()
                        Next
                    Next
                Next
            Next

            'Day17_DisplayGrid(grid, time)
        Next
        Return grid.Sum(Function(f) f.Value.Sum(Function(g) g.Value.Sum(Function(h) h.Value.LongCount(Function(e) e.Value.CurrentState))))
    End Function

    Public Sub Day17_DisplayGrid(grid As Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point))), step1 As Integer)
        Dim Zmin = grid.Values.Min(Function(f) f.Values.Min(Function(g) g.Keys.Min))
        Dim Zmax = grid.Values.Max(Function(f) f.Values.Max(Function(g) g.Keys.Max))
        Dim Ymin = grid.Values.Min(Function(f) f.Keys.Min)
        Dim Ymax = grid.Values.Max(Function(f) f.Keys.Max)
        Dim Xmin = grid.Keys.Min
        Dim Xmax = grid.Keys.Max

        Console.WriteLine("Step " & step1 & ":")
        For i = Zmin To Zmax
            Console.WriteLine("Z= " & i & ":")
            For x = Xmin To Xmax
                If grid.ContainsKey(x) = False Then
                    Dim line As String = ""
                    For y = Ymin To Ymax
                        line &= "."c
                    Next
                    Console.Write(line)
                Else

                    Dim line As String = ""
                    For y = Ymin To Ymax
                        If grid(x).ContainsKey(y) = False Then
                            line += "."c
                        Else
                            If grid(x)(y).ContainsKey(i) = False Then
                                line += "."c
                            Else
                                line += If(grid(x)(y)(i).CurrentState, "#"c, "."c)
                            End If
                        End If

                    Next
                    Console.WriteLine(line)
                End If
            Next
        Next
    End Sub

    Public Function Day17_ReadInput(path As String) As Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point))) 'x,y,z
        Dim result As New Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point)))

        Dim sr As New IO.StreamReader(path)
        Dim x As Integer = 0
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            result.Add(x, New Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point)))
            Dim y As Integer = 0
            For Each c In line.ToCharArray
                result(x).Add(y, New Dictionary(Of Integer, Day17_Grid_Point))
                Select Case c
                    Case "."c
                        result(x)(y).Add(0, New Day17_Grid_Point(False))
                    Case "#"c
                        result(x)(y).Add(0, New Day17_Grid_Point(True))
                    Case Else
                        Throw New NotImplementedException
                End Select
                y = y + 1
            Next
            x = x + 1
        End While

        Return result
    End Function

    Public Function Day17_ReadInput2(path As String) As Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point)))) 'x,y,z,w
        Dim result As New Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point))))

        Dim sr As New IO.StreamReader(path)
        Dim x As Integer = 0
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            result.Add(x, New Dictionary(Of Integer, Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point))))
            Dim y As Integer = 0
            For Each c In line.ToCharArray
                result(x).Add(y, New Dictionary(Of Integer, Dictionary(Of Integer, Day17_Grid_Point)))
                Select Case c
                    Case "."c
                        result(x)(y).Add(0, New Dictionary(Of Integer, Day17_Grid_Point))
                        result(x)(y)(0).Add(0, New Day17_Grid_Point(False))
                    Case "#"c
                        result(x)(y).Add(0, New Dictionary(Of Integer, Day17_Grid_Point))
                        result(x)(y)(0).Add(0, New Day17_Grid_Point(True))
                    Case Else
                        Throw New NotImplementedException
                End Select
                y = y + 1
            Next
            x = x + 1
        End While

        Return result
    End Function


    Public Class Day17_Grid_Point
        Public CurrentState As Boolean
        Public NextState As Boolean
        Public ActiveNeighbours As Integer

        Public Sub New()
            CurrentState = False
            NextState = False
            ActiveNeighbours = 0
        End Sub

        Public Sub New(state As Boolean)
            CurrentState = state
            NextState = state
            ActiveNeighbours = 0
        End Sub

        Public Sub ChangeState()
            CurrentState = NextState
            ActiveNeighbours = 0
        End Sub
    End Class

    'Public Class Day17_Point3D
    '    Public X As Integer
    '    Public Y As Integer
    '    Public Z As Integer

    '    Public Sub New()
    '        X = 0
    '        Y = 0
    '        Z = 0
    '    End Sub

    '    Public Sub New(x As Integer, y As Integer, z As Integer)
    '        Me.X = x
    '        Me.Y = y
    '        Me.Z = z
    '    End Sub

    '    Public Overrides Function ToString() As String
    '        Return X.ToString & " - " & Y.ToString & " - " & Z.ToString
    '    End Function
    'End Class
End Module
