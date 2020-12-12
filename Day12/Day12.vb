Imports System.Drawing

Module Day12
    Public Sub Day12_main()
        Dim testpath = "Day12\Day12_test01.txt"
        Dim inputpath = "Day12\Day12_input.txt"

        Dim testinput = Day12_ReadInput(testpath)
        Dim input = Day12_ReadInput(inputpath)

        Debug.Assert(Day12_Part01(testinput) = 25, "Day12 Part1 test1 failed")
        Console.WriteLine("Day12 Part1: " & Day12_Part01(input))

        Debug.Assert(Day12_Part02(testinput) = 286, "Day12 Part2 test1 failed")
        Console.WriteLine("Day12 Part2: " & Day12_Part02(input))
    End Sub


    Public Function Day12_Part02(instructions As List(Of Day12_Instruction)) As Integer
        'O-> y
        '|   N
        'v W + E
        'x   S
        Dim position As New Drawing.Point(0, 0)
        Dim waypoint As New Drawing.Point(-1, 10) 'represent the waypoint

        For Each inst In instructions
            Select Case inst.Direction
                Case "N"
                    waypoint.X -= inst.Value
                Case "S"
                    waypoint.X += inst.Value
                Case "E"
                    waypoint.Y += inst.Value
                Case "W"
                    waypoint.Y -= inst.Value
                Case "F"
                    position.X += waypoint.X * inst.Value
                    position.Y += waypoint.Y * inst.Value
                Case "L"
                    Select Case inst.Value
                        Case 90
                            waypoint = RotateLeft(waypoint)
                        Case 180
                            waypoint = RotateLeft(waypoint)
                            waypoint = RotateLeft(waypoint)
                        Case 270
                            waypoint = RotateLeft(waypoint)
                            waypoint = RotateLeft(waypoint)
                            waypoint = RotateLeft(waypoint)
                        Case Else
                            Throw New NotImplementedException
                    End Select
                Case "R"
                    Select Case inst.Value
                        Case 90
                            waypoint = RotateLeft(waypoint)
                            waypoint = RotateLeft(waypoint)
                            waypoint = RotateLeft(waypoint)
                        Case 180
                            waypoint = RotateLeft(waypoint)
                            waypoint = RotateLeft(waypoint)
                        Case 270
                            waypoint = RotateLeft(waypoint)
                        Case Else
                            Throw New NotImplementedException
                    End Select
            End Select
        Next

        Return Math.Abs(position.X) + Math.Abs(position.Y)
    End Function


    Public Function Day12_Part01(instructions As List(Of Day12_Instruction)) As Integer
        'O-> y
        '|   N
        'v W + E
        'x   S
        Dim position As New Drawing.Point(0, 0)
        Dim direction As New Drawing.Point(0, 1)

        For Each inst In instructions
            Select Case inst.Direction
                Case "N"
                    position.X -= inst.Value
                Case "S"
                    position.X += inst.Value
                Case "E"
                    position.Y += inst.Value
                Case "W"
                    position.Y -= inst.Value
                Case "F"
                    position.X += direction.X * inst.Value
                    position.Y += direction.Y * inst.Value
                Case "L"
                    Select Case inst.Value
                        Case 90
                            direction = RotateLeft(direction)
                        Case 180
                            direction = RotateLeft(direction)
                            direction = RotateLeft(direction)
                        Case 270
                            direction = RotateLeft(direction)
                            direction = RotateLeft(direction)
                            direction = RotateLeft(direction)
                        Case Else
                            Throw New NotImplementedException
                    End Select
                Case "R"
                    Select Case inst.Value
                        Case 90
                            direction = RotateLeft(direction)
                            direction = RotateLeft(direction)
                            direction = RotateLeft(direction)
                        Case 180
                            direction = RotateLeft(direction)
                            direction = RotateLeft(direction)
                        Case 270
                            direction = RotateLeft(direction)
                        Case Else
                            Throw New NotImplementedException
                    End Select
            End Select
        Next

        Return Math.Abs(position.X) + Math.Abs(position.Y)
    End Function

    Private Function RotateLeft(direction As Point) As Point
        '0,1 -> -1,0
        '-1,0 -> 0,-1
        '0,-1 -> 1,0
        '1,0 -> 0,1

        Dim result As New Drawing.Point(0, 0)
        result.X = -1 * direction.Y
        result.Y = 1 * direction.X
        Return result
    End Function

    Public Function Day12_ReadInput(path As String) As List(Of Day12_Instruction)
        Dim result As New List(Of Day12_Instruction)

        Dim sr = New IO.StreamReader(path)
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            Dim inst = New Day12_Instruction
            inst.Direction = line.First
            inst.Value = Convert.ToInt32(line.Substring(1))
            result.Add(inst)
        End While

        Return result
    End Function

    Public Class Day12_Instruction
        Public Direction As Char
        Public Value As Integer
    End Class
End Module
