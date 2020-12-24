Module Day24

    Public Directions As New List(Of Drawing.Point) From {New Drawing.Point(0, 2), New Drawing.Point(0, -2), New Drawing.Point(2, 1), New Drawing.Point(2, -1), New Drawing.Point(-2, 1), New Drawing.Point(-2, -1)}
    Public Sub Day24_main()
        Dim testpath = "Day24\Day24_test01.txt"
        Dim inputpath = "Day24\Day24_input.txt"

        Dim testinput = Day24_ReadInput(testpath)
        Dim input = Day24_ReadInput(inputpath)

        Debug.Assert(Day24_Part01(testinput) = 10, "Day24 Part1 test1 failed")
        Console.WriteLine("Day24 Part1: " & Day24_Part01(input))

        Debug.Assert(Day24_Part02(testinput) = 2208, "Day24 Part2 test1 failed")
        Console.WriteLine("Day24 Part2: " & Day24_Part02(input))
    End Sub

    Public Function Day24_Part02(input As List(Of Day24_Tile)) As Integer
        Dim tileMap As New Dictionary(Of Drawing.Point, Day24_GoL)

        For Each tile In input
            If tileMap.ContainsKey(tile.Position) Then
                tileMap(tile.Position).CurrentColor = Not (tileMap(tile.Position).CurrentColor)
            Else
                tileMap.Add(tile.Position, New Day24_GoL(True))
            End If
        Next

        For i = 1 To 100
            Dim tilemapcopy As New Dictionary(Of Drawing.Point, Day24_GoL)(tileMap)
            'calculate next color
            For Each tile In tilemapcopy
                If tile.Value.CurrentColor = True Then
                    For Each Dir1 In Directions
                        Dim newpos As New Drawing.Point(tile.Key.X + Dir1.X, tile.Key.Y + Dir1.Y)
                        If tileMap.ContainsKey(newpos) = False Then tileMap.Add(newpos, New Day24_GoL(False))
                        tileMap(newpos).AdjacentBlackNeighbour += 1
                    Next
                End If
            Next

            'calulcate next value
            For Each tile In tileMap
                tile.Value.ChangeState()
            Next
        Next

        Return tileMap.LongCount(Function(f) f.Value.CurrentColor = True)
    End Function



    Public Function Day24_Part01(input As List(Of Day24_Tile)) As Integer
        Dim tilesNumber As New Dictionary(Of Drawing.Point, Integer)

        For Each tile In input
            If tilesNumber.ContainsKey(tile.Position) Then
                tilesNumber(tile.Position) += 1
            Else
                tilesNumber.Add(tile.Position, 1)
            End If
        Next

        Return tilesNumber.LongCount(Function(f) f.Value Mod 2 = 1)
    End Function


    Public Function Day24_ReadInput(path As String) As List(Of Day24_Tile)
        Dim result As New List(Of Day24_Tile)

        Dim sr As New IO.StreamReader(path)
        While Not sr.EndOfStream
            result.Add(New Day24_Tile(sr.ReadLine))
        End While

        Return result
    End Function

    Public Class Day24_GoL
        Public CurrentColor As Boolean = False ' false= white, true = black
        Public AdjacentBlackNeighbour As Integer = 0

        Public Sub New(color As Boolean)
            CurrentColor = color
            AdjacentBlackNeighbour = 0
        End Sub

        Public Sub ChangeState()
            If CurrentColor Then
                'currently black
                If AdjacentBlackNeighbour = 0 OrElse AdjacentBlackNeighbour > 2 Then
                    CurrentColor = False
                Else
                    CurrentColor = True
                End If
            Else
                'currently white
                If AdjacentBlackNeighbour = 2 Then
                    CurrentColor = True
                Else
                    CurrentColor = False
                End If
            End If

            AdjacentBlackNeighbour = 0
        End Sub
    End Class

    Public Class Day24_Tile
        'Public Identification As List(Of Integer)
        Private _Position As Drawing.Point = Nothing

        Public Sub New(s As String)
            'Identification = New List(Of Integer)
            _Position = New Drawing.Point(0, 0)
            For i = 0 To s.Length - 1
                Select Case s(i)
                    Case "e"c
                        _Position.Y += 2
                    Case "w"c
                        _Position.Y -= 2
                    Case "s"c
                        _Position.Y += If(s(i + 1) = "e"c, 1, -1)
                        _Position.X += 2
                        i += 1
                    Case "n"c
                        _Position.Y += If(s(i + 1) = "e"c, 1, -1)
                        _Position.X -= 2
                        i += 1
                End Select
            Next
        End Sub

        Public ReadOnly Property Position As Drawing.Point
            Get
                Return _Position
            End Get
        End Property



    End Class

End Module
