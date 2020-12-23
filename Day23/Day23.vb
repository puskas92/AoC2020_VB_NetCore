Module Day23
    Public Sub Day23_main()
        Debug.Assert(Day23_Part01("389125467", 10) = "92658374", "Day23 Part1 test1 failed")
        Debug.Assert(Day23_Part01("389125467", 100) = "67384529", "Day23 Part1 test2 failed")
        Console.WriteLine("Day23 Part1: " & Day23_Part01("167248359", 100))

        Debug.Assert(Day23_Part02("389125467", 10000000) = 149245887792, "Day23 Part2 test1 failed")
        Console.WriteLine("Day23 Part2: " & Day23_Part02("167248359", 10000000))
    End Sub

    Public Function Day23_Part02(input As String, cycles As Integer) As Int64
        Dim Cups As New LinkedList(Of Integer)(input.ToList.ConvertAll(Function(f) Convert.ToInt32(f.ToString)))
        Dim MaxCupValue = Cups.Max

        For i = MaxCupValue + 1 To 1000000
            Cups.AddLast(i)
        Next

        Day23_PlayCrabGame(cycles, Cups)

        Dim result As Int64 = 1
        Dim currentelement = Cups.Find(1)
        For i = 1 To 2
            currentelement = If(currentelement.Next, Cups.First)
            result *= currentelement.Value
        Next
        Return result
    End Function

    Public Function Day23_Part01(input As String, cycles As Integer) As String
        Dim Cups As New LinkedList(Of Integer)(input.ToList.ConvertAll(Function(f) Convert.ToInt32(f.ToString)))

        Day23_PlayCrabGame(cycles, Cups)

        Dim result As String = ""
        Dim currentelement = Cups.Find(1)
        Do
            currentelement = If(currentelement.Next, Cups.First)
            result += currentelement.Value.ToString
        Loop Until currentelement.Value = 1
        result = result.Replace("1", "")
        Return result
    End Function

    Private Sub Day23_PlayCrabGame(cycles As Integer, Cups As LinkedList(Of Integer))
        Dim MaxCupValue = Cups.Max

        Dim currentcup = Cups.First
        Dim destCup = Cups.First
        'Dim previ As Integer = 0

        Dim cups2 As New List(Of LinkedListNode(Of Integer))
        cups2.Add(Nothing)
        'first 9 is in random order (input), so find their nodes
        For i = 1 To 10
            cups2.Add(Cups.Find(i))
        Next
        'after node 10 the nods are in correct order
        Dim lastCup As LinkedListNode(Of Integer) = cups2(10)
        For i = 11 To Cups.Count
            lastCup = lastCup.Next
            cups2.Add(lastCup)
        Next


        For i = 1 To cycles
            'remove 3 cups
            Dim threecups As New List(Of Integer)
            For j = 1 To 3
                Dim nextCup = If(currentcup.Next, Cups.First)
                threecups.Add(nextCup.Value)
                Cups.Remove(nextCup)
            Next

            'select destination cup
            Dim value = currentcup.Value - 1
            If value = 0 Then value = MaxCupValue
            While threecups.Contains(value)
                value = value - 1
                If value = 0 Then value = MaxCupValue
            End While
            'Dim destCup = Cups.Find(value)
            'destCup = find1.Find(value)
            destCup = cups2(value)

            'place the numbers after the dest cup
            For Each cup In threecups
                destCup = Cups.AddAfter(destCup, cup)
                'update these cups in list
                cups2(destCup.Value) = destCup
            Next

            'set next current cup
            currentcup = If(currentcup.Next, Cups.First)

            ''displaypercent
            'If Math.Round(i / (cycles / 100), 0) > previ Then
            '    previ = Math.Round(i / (cycles / 100), 0)
            '    Console.WriteLine(Date.Now.ToShortTimeString & " : " & previ.ToString & " %")
            'End If
        Next
    End Sub
End Module
