Module Day22
    Public Sub Day22_main()
        Dim testpath = "Day22\Day22_test01.txt"
        Dim inputpath = "Day22\Day22_input.txt"

        Dim testinput = Day22_ReadInput(testpath)
        Dim input = Day22_ReadInput(inputpath)

        Debug.Assert(Day22_Part01(testinput) = 306, "Day22 Part1 test1 failed")
        Console.WriteLine("Day22 Part1: " & Day22_Part01(input))

        'read again to have the same starting deck
        testinput = Day22_ReadInput(testpath)
        input = Day22_ReadInput(inputpath)

        Dim testpath2 = "Day22\Day22_test02.txt"
        Dim testinput2 = Day22_ReadInput(testpath2)

        Debug.Assert(Day22_Part02(testinput) = 291, "Day22 Part2 test1 failed")
        Debug.Assert(Day22_Part02(testinput2) = 105, "Day22 Part2 test1 failed")
        Console.WriteLine("Day22 Part2: " & Day22_Part02(input))
    End Sub

    Public Function Day22_Part02(input As Day22_Input) As Integer
        Dim winnerdeck As Queue(Of Integer)
        If PlayRecursive(input) = 1 Then
            'player1 wins
            winnerdeck = input.Player1_Deck
        Else
            'player2 wins
            winnerdeck = input.Player2_Deck
        End If

        Dim result As Integer = 0
        For i = winnerdeck.Count To 1 Step -1
            result += i * winnerdeck.Dequeue
        Next

        Return result
    End Function

    Public Function PlayRecursive(input As Day22_Input) As Integer
        Dim gamecache As New List(Of Tuple(Of Integer, Integer))
        Dim winner As Integer = 0
        While input.Player1_Deck.Count > 0 AndAlso input.Player2_Deck.Count > 0
            'check first if same deck was present, hope hascode is enough
            Dim tup = New Tuple(Of Integer, Integer)(GetSequenceHashCode(input.Player1_Deck.ToList), GetSequenceHashCode(input.Player2_Deck.ToList))
            If gamecache.Contains(tup) Then
                winner = 1
                Exit While
            Else
                gamecache.Add(tup)
            End If
            'draw 1 card
            Dim player1_card = input.Player1_Deck.Dequeue
            Dim player2_card = input.Player2_Deck.Dequeue

            'check if Recursive Combat should be played
            If input.Player1_Deck.Count >= player1_card AndAlso input.Player2_Deck.Count >= player2_card AndAlso input.Player1_Deck.Count >= 1 AndAlso input.Player2_Deck.Count >= 1 Then
                'recusive combat
                Dim input2 As New Day22_Input
                input2.Player1_Deck = New Queue(Of Integer)(input.Player1_Deck.ToList.GetRange(0, player1_card)) 'top card already removed, selecting first cards as the number of the current card
                input2.Player2_Deck = New Queue(Of Integer)(input.Player2_Deck.ToList.GetRange(0, player2_card))
                winner = PlayRecursive(input2)
                'Continue While
            Else
                'normal game
                winner = If(player1_card > player2_card, 1, 2)
            End If


            If winner = 1 Then
                input.Player1_Deck.Enqueue(player1_card)
                input.Player1_Deck.Enqueue(player2_card)
            Else
                input.Player2_Deck.Enqueue(player2_card)
                input.Player2_Deck.Enqueue(player1_card)
            End If

        End While

        Return winner
    End Function

    Public Function Day22_Part01(input As Day22_Input) As Integer
        While input.Player1_Deck.Count > 0 AndAlso input.Player2_Deck.Count > 0
            Dim player1_card = input.Player1_Deck.Dequeue
            Dim player2_card = input.Player2_Deck.Dequeue

            If player1_card > player2_card Then
                input.Player1_Deck.Enqueue(player1_card)
                input.Player1_Deck.Enqueue(player2_card)
            Else
                input.Player2_Deck.Enqueue(player2_card)
                input.Player2_Deck.Enqueue(player1_card)
            End If
        End While


        Dim winnerdeck As Queue(Of Integer)
        If input.Player1_Deck.Count > 0 Then
            'player1 wins
            winnerdeck = input.Player1_Deck
        Else
            'player2 wins
            winnerdeck = input.Player2_Deck
        End If

        Dim result As Integer = 0
        For i = winnerdeck.Count To 1 Step -1
            result += i * winnerdeck.Dequeue
        Next

        Return result
    End Function

    Public Function Day22_ReadInput(path As String) As Day22_Input
        Dim result As New Day22_Input

        Dim sr As New IO.StreamReader(path)
        'player1
        Dim helperList As New List(Of Integer)
        sr.ReadLine() 'Player 1:
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            If line = "" Then Exit While
            helperList.Add(Convert.ToInt32(line))
        End While
        result.Player1_Deck = New Queue(Of Integer)(helperList)

        helperList = New List(Of Integer)
        sr.ReadLine() 'Player 2:
        While Not sr.EndOfStream
            helperList.Add(Convert.ToInt32(sr.ReadLine))
        End While
        result.Player2_Deck = New Queue(Of Integer)(helperList)

        Return result
    End Function

    Public Function GetSequenceHashCode(Of T)(sequence As IList(Of T)) As Integer
        Dim seed As Integer = 487
        Dim modifier As Integer = 31

        Dim result As Integer = seed
        For Each item In sequence
            Dim subresult As Int64
            subresult = ((Convert.ToInt64(result) * Convert.ToInt64(modifier)) + Convert.ToInt64(item.GetHashCode))
            result = subresult Mod Integer.MaxValue
        Next
        Return result
    End Function


    Public Class Day22_Input
        Public Player1_Deck As Queue(Of Integer)
        Public Player2_Deck As Queue(Of Integer)
    End Class
End Module
