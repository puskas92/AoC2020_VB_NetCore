Module Day20
    Public Const Day20_TileSizeMinusOne As Integer = 9
    Public Sub Day20_main()
        Dim testpath = "Day20\Day20_test01.txt"
        Dim inputpath = "Day20\Day20_input.txt"

        Dim testinput = Day20_ReadInput(testpath)
        Dim input = Day20_ReadInput(inputpath)

        Debug.Assert(Day20_Part01(testinput) = 20899048083289, "Day20 Part1 test1 failed")
        Console.WriteLine("Day20 Part1: " & Day20_Part01(input))

        Debug.Assert(Day20_Part02(testinput) = 273, "Day20 Part2 test1 failed")
        Console.WriteLine("Day20 Part2: " & Day20_Part02(input))
    End Sub

    Public Function Day20_Part02(input As Dictionary(Of Integer, Day20_Tiles)) As UInt64
        Dim picture As New List(Of List(Of Integer))
        Dim gridsize As Integer = Math.Round(Math.Sqrt(input.Count))
        For i = 0 To gridsize - 1
            picture.Add(New List(Of Integer))
            For j = 0 To gridsize - 1
                picture(i).Add(0)
            Next
        Next

        'get a cornertile 
        Dim CornerTile = input.ToList.Find(Function(f) f.Value.MatchesWith.Count = 2).Value
        'get the two connected tiles
        Dim CornerMatchesWith = CornerTile.MatchesWith.Keys
        Dim CornerConnectedTile1 = input(CornerMatchesWith(0))
        Dim CornerConnectedTile2 = input(CornerMatchesWith(1))
        'assume
        picture(0)(0) = CornerTile.Id
        picture(0)(1) = CornerConnectedTile1.Id
        picture(1)(0) = CornerConnectedTile2.Id

        'brute force the starting orientation of the first 3 tiles
        Dim found As Boolean = False
        For i = 0 To 8
            CornerTile.IsMirrored = ((i / 4) >= 1)
            CornerTile.RotateToLeft = i Mod 4
            For j = 0 To 8
                CornerConnectedTile1.IsMirrored = ((j / 4) >= 1)
                CornerConnectedTile1.RotateToLeft = j Mod 4
                If CornerTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.RightEdge) <> CornerConnectedTile1.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.LeftEdge) Then Continue For
                For k = 0 To 8
                    CornerConnectedTile2.IsMirrored = ((k / 4) >= 1)
                    CornerConnectedTile2.RotateToLeft = k Mod 4
                    If CornerTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.LowerEdge) = CornerConnectedTile2.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.UpperEdge) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = True Then Exit For
            Next
            If found = True Then Exit For
        Next

        For i = 2 To ((2 * gridsize) - 2)
            'iterate trough diagonally

            'inner tiles
            For j = 0 To i
                'find next
                Dim x = i - j
                Dim y = j

                If x < 0 OrElse x > gridsize - 1 OrElse y < 0 OrElse y > gridsize - 1 Then 'outside of grid
                    Continue For
                End If

                If i < gridsize AndAlso (x = 0 OrElse y = 0) Then 'edges should be considered differently
                    'should check it after the inner cells are calcualted
                    Continue For
                Else
                    Dim leftTileId = picture(x)(y - 1)
                    Dim upperTileId = picture(x - 1)(y)
                    Dim leftupperTileId = picture(x - 1)(y - 1)
                    Dim leftTile = input(leftTileId)
                    Dim upperTile = input(upperTileId)

                    Dim intersect = leftTile.MatchesWith.Keys.Intersect(upperTile.MatchesWith.Keys).ToList
                    If intersect.Contains(leftupperTileId) Then
                        intersect.Remove(leftupperTileId)
                    End If

                    Debug.Assert(intersect.Count = 1, "More than one tile possible")

                    picture(x)(y) = intersect.First
                    Dim currentTile = input(picture(x)(y))

                    'find correct orienation of picture(x)(y)
                    For k = 0 To 8
                        currentTile.IsMirrored = ((k / 4) >= 1)
                        currentTile.RotateToLeft = k Mod 4
                        If upperTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.LowerEdge) = currentTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.UpperEdge) AndAlso leftTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.RightEdge) = currentTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.LeftEdge) Then
                            Exit For
                        End If
                        Debug.Assert(k <> 8, "correct oreination not found")
                    Next
                End If
            Next

            If i < gridsize Then

                Dim x = 0
                Dim y = i
                Dim leftTileId = picture(x)(y - 1)
                Dim leftTile = input(leftTileId)
                Dim leftDownId = picture(x + 1)(y - 1)
                Dim leftLeftId = picture(x)(y - 2)

                Dim possibles = leftTile.MatchesWith.Keys.ToList
                If possibles.Contains(leftDownId) Then
                    possibles.Remove(leftDownId)
                End If
                If possibles.Contains(leftLeftId) Then
                    possibles.Remove(leftLeftId)
                End If

                Debug.Assert(possibles.Count = 1, "More than one tile possible")

                picture(x)(y) = possibles.First
                Dim currentTile = input(picture(x)(y))

                'find correct orienation of picture(x)(y) 
                For k = 0 To 8
                    currentTile.IsMirrored = ((k / 4) >= 1)
                    currentTile.RotateToLeft = k Mod 4
                    If leftTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.RightEdge) = currentTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.LeftEdge) Then
                        Exit For
                    End If
                    Debug.Assert(k <> 8, "correct oreination not found")
                Next


                'other edge
                x = i
                y = 0

                Dim upperTileId = picture(x - 1)(y)
                Dim upperTile = input(upperTileId)

                Dim upperrightId = picture(x - 1)(y + 1)
                Dim upperupperId = picture(x - 2)(y)

                Dim possibles2 = upperTile.MatchesWith.Keys.ToList
                If possibles2.Contains(upperrightId) Then
                    possibles2.Remove(upperrightId)
                End If
                If possibles2.Contains(upperupperId) Then
                    possibles2.Remove(upperupperId)
                End If

                Debug.Assert(possibles2.Count = 1, "More than one tile possible")

                picture(x)(y) = possibles2.First
                Dim currentTile2 = input(picture(x)(y))

                'find correct orienation of picture(x)(y)
                For k = 0 To 8
                    currentTile2.IsMirrored = ((k / 4) >= 1)
                    currentTile2.RotateToLeft = k Mod 4
                    If upperTile.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.LowerEdge) = currentTile2.PossibleEdgesAtCurrentOrientation()(Day20_Tiles.Edges.UpperEdge) Then
                        Exit For
                    End If
                    Debug.Assert(k <> 8, "correct oreination not found")
                Next
            End If
        Next

        'tiles created, create big picture
        Dim BigPicture As New List(Of List(Of Day20_BigPicturePixel))
        For i = 0 To (gridsize * (Day20_TileSizeMinusOne - 1)) - 1
            BigPicture.Add(New List(Of Day20_BigPicturePixel))
            For j = 0 To (gridsize * (Day20_TileSizeMinusOne - 1)) - 1
                BigPicture(i).Add(New Day20_BigPicturePixel)
            Next
        Next


        For i = 0 To picture.Count - 1 'Each RowOfTiles In picture
            For j = 0 To picture(i).Count - 1 'Each TileId In RowOfTiles
                Dim tile = input(picture(i)(j))
                Dim tilelowPicture = tile.PictureAtDirection

                For k = 0 To tilelowPicture.Count - 1 'Each RowOfPixels In tilelowPicture
                    For l = 0 To tilelowPicture(k).Count - 1 'Each Pixel In RowOfPixels
                        BigPicture(i * (Day20_TileSizeMinusOne - 1) + k)(j * (Day20_TileSizeMinusOne - 1) + l).Value = tilelowPicture(k)(l)
                        BigPicture(i * (Day20_TileSizeMinusOne - 1) + k)(j * (Day20_TileSizeMinusOne - 1) + l).Type = If(tilelowPicture(k)(l), 1, 0)
                    Next
                Next
            Next
        Next

        Dim sr As New IO.StreamReader("Day20\Day20_input_seamonster.txt")
        Dim SeaMonst As New Day20_SeaMonster(sr.ReadToEnd)

        For i = 0 To BigPicture.Count - 1 - SeaMonst.MaxX
            For j = 0 To BigPicture.Count - 1 - SeaMonst.MaxY
                For k = 0 To 3
                    Dim valid As Boolean = True
                    For Each pix In SeaMonst.pixels
                        Dim x As Integer = 0
                        Dim y As Integer = 0
                        Select Case k
                            Case 0
                                x = i + pix.X
                                y = j + pix.Y
                            Case 1
                                x = i + SeaMonst.MaxX - 1 - pix.X
                                y = j + pix.Y
                            Case 2
                                x = i + pix.X
                                y = j + SeaMonst.MaxY - 1 - pix.Y
                            Case 3
                                x = i + SeaMonst.MaxX - 1 - pix.X
                                y = j + SeaMonst.MaxY - 1 - pix.Y
                        End Select
                        If BigPicture(x)(y).Value = False Then
                            valid = False
                            Exit For
                        End If
                    Next
                    If valid Then
                        For Each pix In SeaMonst.pixels
                            Dim x As Integer = 0
                            Dim y As Integer = 0
                            Select Case k
                                Case 0
                                    x = i + pix.X
                                    y = j + pix.Y
                                Case 1
                                    x = i + SeaMonst.MaxX - 1 - pix.X
                                    y = j + pix.Y
                                Case 2
                                    x = i + pix.X
                                    y = j + SeaMonst.MaxY - 1 - pix.Y
                                Case 3
                                    x = i + SeaMonst.MaxX - 1 - pix.X
                                    y = j + SeaMonst.MaxY - 1 - pix.Y
                            End Select
                            BigPicture(x)(y).Type = 2
                        Next
                        'For m = 0 To SeaMonst.MaxX
                        '    For l = 0 To SeaMonst.MaxY
                        '        Dim x As Integer = 0
                        '        Dim y As Integer = 0
                        '        Select Case k
                        '            Case 0
                        '                x = i + m
                        '                y = j + l
                        '            Case 1
                        '                x = i + SeaMonst.MaxX - 1 - m
                        '                y = j + l
                        '            Case 2
                        '                x = i + m
                        '                y = j + SeaMonst.MaxY - 1 - l
                        '            Case 3
                        '                x = i + SeaMonst.MaxX - 1 - m
                        '                y = j + SeaMonst.MaxY - 1 - l
                        '        End Select
                        '        If BigPicture(x)(y).Type = 1 Then
                        '            BigPicture(x)(y).Type = 3
                        '        End If
                        '    Next
                        'Next
                    End If
                Next
            Next
        Next

        For i = 0 To BigPicture.Count - 1 - SeaMonst.MaxY
            For j = 0 To BigPicture.Count - 1 - SeaMonst.MaxX
                For k = 0 To 3
                    Dim valid As Boolean = True
                    For Each pix In SeaMonst.pixels
                        Dim x As Integer = 0
                        Dim y As Integer = 0
                        Select Case k
                            Case 0
                                x = i + pix.Y
                                y = j + pix.X
                            Case 1
                                x = i + (SeaMonst.MaxY - 1) - pix.Y
                                y = j + pix.X
                            Case 2
                                x = i + pix.Y
                                y = j + (SeaMonst.MaxX - 1) - pix.X
                            Case 3
                                x = i + (SeaMonst.MaxY - 1) - pix.Y
                                y = j + (SeaMonst.MaxX - 1) - pix.X
                        End Select
                        If BigPicture(x)(y).Value = False Then
                            valid = False
                            Exit For
                        End If
                    Next
                    If valid Then
                        For Each pix In SeaMonst.pixels
                            Dim x As Integer = 0
                            Dim y As Integer = 0
                            Select Case k
                                Case 0
                                    x = i + pix.Y
                                    y = j + pix.X
                                Case 1
                                    x = i + (SeaMonst.MaxY - 1) - pix.Y
                                    y = j + pix.X
                                Case 2
                                    x = i + pix.Y
                                    y = j + (SeaMonst.MaxX - 1) - pix.X
                                Case 3
                                    x = i + (SeaMonst.MaxY - 1) - pix.Y
                                    y = j + (SeaMonst.MaxX - 1) - pix.X
                            End Select
                            BigPicture(x)(y).Type = 2
                        Next
                        'For m = 0 To SeaMonst.MaxX
                        '    For l = 0 To SeaMonst.MaxY
                        '        Dim x As Integer = 0
                        '        Dim y As Integer = 0
                        '        Select Case k
                        '            Case 0
                        '                x = i + l
                        '                y = j + m
                        '            Case 1
                        '                x = i + (SeaMonst.MaxY - 1) - l
                        '                y = j + m
                        '            Case 2
                        '                x = i + l
                        '                y = j + (SeaMonst.MaxX - 1) - m
                        '            Case 3
                        '                x = i + (SeaMonst.MaxY - 1) - l
                        '                y = j + (SeaMonst.MaxX - 1) - m
                        '        End Select
                        '        If BigPicture(x)(y).Type = 1 Then
                        '            BigPicture(x)(y).Type = 3
                        '        End If
                        '    Next
                        'Next
                    End If
                Next
            Next
        Next

        'Day20_DisplayPicture(BigPicture)

        Return BigPicture.Sum(Function(f) f.LongCount(Function(g) g.Type = 3 OrElse g.Type = 1))
    End Function


    Public Sub Day20_DisplayPicture(pic As List(Of List(Of Day20_BigPicturePixel)))
        For Each row In pic
            For Each pix In row
                Select Case pix.Type
                    Case 0, 1
                        Console.ForegroundColor = ConsoleColor.White
                    Case 2
                        Console.ForegroundColor = ConsoleColor.Red
                    Case 3
                        Console.ForegroundColor = ConsoleColor.Blue
                End Select
                Console.Write(If(pix.Value, "#"c, "."c))
            Next
            Console.WriteLine("")
        Next
    End Sub

    Public Class Day20_BigPicturePixel
        Public Value As Boolean
        Public Type As Integer '0 - empty, 1-full, 2-monster, 3-water around monster

        Public Sub New()
            Value = False
            Type = 0
        End Sub
    End Class

    Public Sub Day20_DisplayTile(tile As Day20_Tiles)
        Dim st As String = "" '= tile.Id.ToString & vbCrLf
        Dim pic = tile.PictureAtDirectionFull
        For Each row In pic
            st += String.Concat(row.ConvertAll(Function(f) If(f, "#"c, "."c)))
            st += vbCrLf
        Next
        Console.WriteLine(st)
    End Sub


    Public Function Day20_Part01(input As Dictionary(Of Integer, Day20_Tiles)) As UInt64
        Dim MathingTiles As New List(Of Tuple(Of Integer, Integer, Integer))
        For i = 0 To input.Count - 2
            For j = i + 1 To input.Count - 1
                Dim tile1 = input.ElementAt(i)
                Dim tile2 = input.ElementAt(j)
                Dim intersect = tile1.Value.PossibleEdges.Intersect(tile2.Value.PossibleEdges)
                For Each inter In intersect
                    MathingTiles.Add(New Tuple(Of Integer, Integer, Integer)(tile1.Key, tile2.Key, inter))
                Next
            Next
        Next

        Dim NumberOfMathces As New Dictionary(Of Integer, Integer)
        For Each tile In input
            tile.Value.MatchesWith = New Dictionary(Of Integer, List(Of Integer))
            For Each match In MathingTiles
                If match.Item1 = tile.Key Then
                    If tile.Value.MatchesWith.ContainsKey(match.Item2) = False Then
                        tile.Value.MatchesWith.Add(match.Item2, New List(Of Integer))
                    End If
                    tile.Value.MatchesWith(match.Item2).Add(match.Item3)
                ElseIf match.Item2 = tile.Key Then
                    If tile.Value.MatchesWith.ContainsKey(match.Item1) = False Then
                        tile.Value.MatchesWith.Add(match.Item1, New List(Of Integer))
                    End If
                    tile.Value.MatchesWith(match.Item1).Add(match.Item3)
                Else
                    'no match
                End If
            Next
        Next

        Dim CornerTiles = input.ToList.FindAll(Function(f) f.Value.MatchesWith.Count = 2)
        Debug.Assert(CornerTiles.Count = 4, "Assert only 4 tiles can connect with only 2 others")
        Dim multi As UInt64 = 1
        For Each corner In CornerTiles
            multi *= corner.Key
        Next
        Return multi
    End Function


    Public Function Day20_ReadInput(path As String) As Dictionary(Of Integer, Day20_Tiles)
        Dim result As New Dictionary(Of Integer, Day20_Tiles)

        Dim sr As New IO.StreamReader(path)
        Dim tile = New Day20_Tiles
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            If line = "" Then
                result.Add(tile.Id, tile)
                tile = New Day20_Tiles
                Continue While
            End If

            If line.Contains("Tile") Then
                tile.Id = Convert.ToInt32(line.Split(" ")(1).Replace(":", "").Trim)
            Else
                tile.Picture.Add(line.ToList.ConvertAll(Function(f) (f = "#"c)))
            End If

        End While
        result.Add(tile.Id, tile)

        Return result
    End Function

    Public Class Day20_Tiles
        Public Id As Integer
        Public Picture As New List(Of List(Of Boolean))
        Public RotateToLeft As Integer = 0
        Public IsMirrored As Boolean = False
        Public MatchesWith As Dictionary(Of Integer, List(Of Integer))
        Private _PossibleEdges As List(Of Integer)
        Public Function PossibleEdges() As List(Of Integer)
            If _PossibleEdges Is Nothing Then
                Dim result As New List(Of Integer)
                For i = 0 To 7
                    result.Add(0) '4 side, and their inverz
                Next

                For i = 0 To Day20_TileSizeMinusOne
                    Dim TwoPower = Math.Pow(2, i)
                    result(0) += If(Picture(i)(0), 1, 0) * TwoPower
                    result(1) += If(Picture(Day20_TileSizeMinusOne - i)(0), 1, 0) * TwoPower
                    result(2) += If(Picture(i)(Day20_TileSizeMinusOne), 1, 0) * TwoPower
                    result(3) += If(Picture(Day20_TileSizeMinusOne - i)(Day20_TileSizeMinusOne), 1, 0) * TwoPower
                    result(4) += If(Picture(0)(i), 1, 0) * TwoPower
                    result(5) += If(Picture(0)(Day20_TileSizeMinusOne - i), 1, 0) * TwoPower
                    result(6) += If(Picture(Day20_TileSizeMinusOne)(i), 1, 0) * TwoPower
                    result(7) += If(Picture(Day20_TileSizeMinusOne)(Day20_TileSizeMinusOne - i), 1, 0) * TwoPower
                Next
                _PossibleEdges = result
            End If

            Return _PossibleEdges
        End Function

        Public Function PictureAtDirection() As List(Of List(Of Boolean))
            Dim result As New List(Of List(Of Boolean))

            Dim row As Integer = 0

            If IsMirrored Then
                Select Case RotateToLeft
                    Case 0
                        For x = 1 To Day20_TileSizeMinusOne - 1
                            result.Add(New List(Of Boolean))
                            For y = Day20_TileSizeMinusOne - 1 To 1 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 1
                        For y = Day20_TileSizeMinusOne - 1 To 1 Step -1
                            result.Add(New List(Of Boolean))
                            For x = Day20_TileSizeMinusOne - 1 To 1 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 2
                        For x = Day20_TileSizeMinusOne - 1 To 1 Step -1
                            result.Add(New List(Of Boolean))
                            For y = 1 To Day20_TileSizeMinusOne - 1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 3
                        For y = 1 To Day20_TileSizeMinusOne - 1
                            result.Add(New List(Of Boolean))
                            For x = 1 To Day20_TileSizeMinusOne - 1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                End Select
            Else
                Select Case RotateToLeft
                    Case 0
                        For x = 1 To Day20_TileSizeMinusOne - 1
                            result.Add(New List(Of Boolean))
                            For y = 1 To Day20_TileSizeMinusOne - 1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 1
                        For y = Day20_TileSizeMinusOne - 1 To 1 Step -1
                            result.Add(New List(Of Boolean))
                            For x = 1 To Day20_TileSizeMinusOne - 1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 2
                        For x = Day20_TileSizeMinusOne - 1 To 1 Step -1
                            result.Add(New List(Of Boolean))
                            For y = Day20_TileSizeMinusOne - 1 To 1 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 3
                        For y = 1 To Day20_TileSizeMinusOne - 1
                            result.Add(New List(Of Boolean))
                            For x = Day20_TileSizeMinusOne - 1 To 1 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                End Select

            End If
            Return result
        End Function

        Public Function PictureAtDirectionFull() As List(Of List(Of Boolean))
            Dim result As New List(Of List(Of Boolean))

            Dim row As Integer = 0

            If IsMirrored Then
                Select Case RotateToLeft
                    Case 0
                        For x = 0 To Day20_TileSizeMinusOne
                            result.Add(New List(Of Boolean))
                            For y = Day20_TileSizeMinusOne To 0 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 1
                        For y = Day20_TileSizeMinusOne To 0 Step -1
                            result.Add(New List(Of Boolean))
                            For x = Day20_TileSizeMinusOne To 0 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 2
                        For x = Day20_TileSizeMinusOne To 0 Step -1
                            result.Add(New List(Of Boolean))
                            For y = 0 To Day20_TileSizeMinusOne
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 3
                        For y = 0 To Day20_TileSizeMinusOne
                            result.Add(New List(Of Boolean))
                            For x = 0 To Day20_TileSizeMinusOne
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                End Select
            Else
                Select Case RotateToLeft
                    Case 0
                        For x = 0 To Day20_TileSizeMinusOne
                            result.Add(New List(Of Boolean))
                            For y = 0 To Day20_TileSizeMinusOne
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 1
                        For y = Day20_TileSizeMinusOne To 0 Step -1
                            result.Add(New List(Of Boolean))
                            For x = 0 To Day20_TileSizeMinusOne
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 2
                        For x = Day20_TileSizeMinusOne To 0 Step -1
                            result.Add(New List(Of Boolean))
                            For y = Day20_TileSizeMinusOne To 0 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                    Case 3
                        For y = 0 To Day20_TileSizeMinusOne
                            result.Add(New List(Of Boolean))
                            For x = Day20_TileSizeMinusOne To 0 Step -1
                                result(row).Add(Picture(x)(y))
                            Next
                            row += 1
                        Next
                End Select

            End If
            Return result
        End Function

        Public Function PossibleEdgesAtCurrentOrientation() As Dictionary(Of Edges, Integer)
            Dim result As New Dictionary(Of Edges, Integer)
            Dim orientedpicture = PictureAtDirectionFull()

            result.Add(Edges.LowerEdge, 0)
            result.Add(Edges.UpperEdge, 0)
            result.Add(Edges.LeftEdge, 0)
            result.Add(Edges.RightEdge, 0)

            For i = 0 To Day20_TileSizeMinusOne
                Dim TwoPower = Math.Pow(2, i)
                result(Edges.LeftEdge) += If(orientedpicture(i)(0), 1, 0) * TwoPower
                'result(1) += If(Picture(Day20_TileSizeMinusOne - i)(0), 1, 0) * TwoPower
                result(Edges.RightEdge) += If(orientedpicture(i)(Day20_TileSizeMinusOne), 1, 0) * TwoPower
                'result(3) += If(Picture(Day20_TileSizeMinusOne - i)(Day20_TileSizeMinusOne), 1, 0) * TwoPower
                result(Edges.UpperEdge) += If(orientedpicture(0)(i), 1, 0) * TwoPower
                'result(5) += If(Picture(0)(Day20_TileSizeMinusOne - i), 1, 0) * TwoPower
                result(Edges.LowerEdge) += If(orientedpicture(Day20_TileSizeMinusOne)(i), 1, 0) * TwoPower
                'result(7) += If(Picture(Day20_TileSizeMinusOne)(Day20_TileSizeMinusOne - i), 1, 0) * TwoPower
            Next

            'If IsMirrored Then
            '    Select Case RotateToLeft
            '        Case 0
            '            result.Add(Edges.UpperEdge, PossibleEdges(4))
            '            result.Add(Edges.LowerEdge, PossibleEdges(6))
            '            result.Add(Edges.LeftEdge, PossibleEdges(2))
            '            result.Add(Edges.RightEdge, PossibleEdges(0))
            '        Case 1
            '            result.Add(Edges.UpperEdge, PossibleEdges(2))
            '            result.Add(Edges.LowerEdge, PossibleEdges(0))
            '            result.Add(Edges.LeftEdge, PossibleEdges(7))
            '            result.Add(Edges.RightEdge, PossibleEdges(5))
            '        Case 2
            '            result.Add(Edges.UpperEdge, PossibleEdges(7))
            '            result.Add(Edges.LowerEdge, PossibleEdges(5))
            '            result.Add(Edges.LeftEdge, PossibleEdges(1))
            '            result.Add(Edges.RightEdge, PossibleEdges(3))
            '        Case 3
            '            result.Add(Edges.UpperEdge, PossibleEdges(1))
            '            result.Add(Edges.LowerEdge, PossibleEdges(3))
            '            result.Add(Edges.LeftEdge, PossibleEdges(4))
            '            result.Add(Edges.RightEdge, PossibleEdges(6))
            '    End Select
            'Else
            '    Select Case RotateToLeft
            '        Case 0
            '            result.Add(Edges.UpperEdge, PossibleEdges(4))
            '            result.Add(Edges.LowerEdge, PossibleEdges(6))
            '            result.Add(Edges.LeftEdge, PossibleEdges(0))
            '            result.Add(Edges.RightEdge, PossibleEdges(2))
            '        Case 1
            '            result.Add(Edges.UpperEdge, PossibleEdges(2))
            '            result.Add(Edges.LowerEdge, PossibleEdges(0))
            '            result.Add(Edges.LeftEdge, PossibleEdges(5))
            '            result.Add(Edges.RightEdge, PossibleEdges(7))
            '        Case 2
            '            result.Add(Edges.UpperEdge, PossibleEdges(7))
            '            result.Add(Edges.LowerEdge, PossibleEdges(5))
            '            result.Add(Edges.LeftEdge, PossibleEdges(3))
            '            result.Add(Edges.RightEdge, PossibleEdges(1))
            '        Case 3
            '            result.Add(Edges.UpperEdge, PossibleEdges(1))
            '            result.Add(Edges.LowerEdge, PossibleEdges(3))
            '            result.Add(Edges.LeftEdge, PossibleEdges(6))
            '            result.Add(Edges.RightEdge, PossibleEdges(4))
            '    End Select

            'End If
            Return result
        End Function

        Public Enum Edges As Integer
            UpperEdge = 0
            LowerEdge = 1
            LeftEdge = 2
            RightEdge = 3
        End Enum
    End Class

    Public Class Day20_SeaMonster
        Public pixels As New List(Of Drawing.Point)
        Public MaxY As Integer = 0
        Public MaxX As Integer = 0
        Public Sub New(input As String)
            Dim lines = input.Split(vbCrLf)
            MaxX = lines.Count
            For i = 0 To MaxX - 1
                MaxY = Math.Max(lines(i).Length, MaxY)
                For j = 0 To MaxY - 1
                    If lines(i)(j) = "#"c Then
                        pixels.Add(New Drawing.Point(i, j))
                    End If
                Next
            Next
        End Sub
    End Class

End Module
