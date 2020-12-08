Module Day07
    Public Sub Day07_main()
        Dim testpath = "Day07\Day07_test01.txt"
        Dim inputpath = "Day07\Day07_input.txt"

        Dim testinput = Day07_ReadInput(testpath)
        Dim input = Day07_ReadInput(inputpath)

        Debug.Assert(Day07_Part01(testinput, "shiny gold") = 4, "Day07 Part1 test1 failed")
        Console.WriteLine("Day07 Part1: " & Day07_Part01(input, "shiny gold"))

        Dim test2path = "Day07\Day07_test02.txt"
        Dim test2input = Day07_ReadInput(test2path)
        Debug.Assert(Day07_Part02(testinput, "shiny gold") - 1 = 32, "Day07 Part2 test1 failed")
        Debug.Assert(Day07_Part02(test2input, "shiny gold") - 1 = 126, "Day07 Part2 test1 failed")
        Console.WriteLine("Day07 Part2: " & Day07_Part02(input, "shiny gold") - 1)  '-1 because it is also counting shiny gold bag itself. I didn't want to make an extra function because of this
    End Sub

    Public Function Day07_Part02(input As List(Of Bag), color As String) As Int64
        Dim bag = input.Find(Function(f) f.Color = color)
        Return 1 + bag.Contain.Sum(Function(f) f.Value * Day07_Part02(input, f.Key))
    End Function

    Public Function Day07_Part01(input As List(Of Bag), color As String) As Integer
        Dim resultColors As Dictionary(Of String, Boolean)
        resultColors = SearchColorInContains(input, color)
        Return resultColors.Count
    End Function

    Public Function SearchColorInContains(input As List(Of Bag), color As String) As Dictionary(Of String, Boolean)
        Dim result As New Dictionary(Of String, Boolean)
        Dim contains = input.FindAll(Function(f) f.Contain.ContainsKey(color))
        For Each contain In contains
            Dim subresult As New Dictionary(Of String, Boolean)
            If result.ContainsKey(contain.Color) = False OrElse result(contain.Color) = False Then
                subresult = SearchColorInContains(input, contain.Color)
            End If

            If result.ContainsKey(contain.Color) Then
                result(contain.Color) = True
            Else
                result.Add(contain.Color, True)
            End If

            For Each subres In subresult
                If result.ContainsKey(subres.Key) = False Then
                    result.Add(subres.Key, subres.Value)
                End If
            Next
        Next

        Return result
    End Function

    Public Function Day07_ReadInput(path As String) As List(Of Bag)
        'light red bags contain 1 bright white bag, 2 muted yellow bags.
        'posh chartreuse bags contain 5 pale indigo bags, 2 striped bronze bags, 3 mirrored blue bags, 1 light cyan bag.
        'faded blue bags contain no other bags.

        Dim result As New List(Of Bag)
        Dim sr As New IO.StreamReader(path)

        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            Dim bag = New Bag

            Dim words = line.Split(" ")
            bag.Color = words(0) & " " & words(1)
            bag.Contain = New Dictionary(Of String, Integer)

            Dim containsstrings = line.Split("contain")(1).Split(",")
            If containsstrings.Length = 1 AndAlso containsstrings(0) = " no other bags." Then
                'no contain
            Else
                For Each containStr In containsstrings
                    Dim containwords = containStr.Trim.Split(" ")
                    Dim number = Convert.ToInt32(containwords(0))
                    Dim color = containwords(1) & " " & containwords(2)

                    bag.Contain.Add(color, number)
                Next
            End If

            result.Add(bag)
        End While

        Return result
    End Function

    Public Class Bag
        Public Color As String
        Public Contain As Dictionary(Of String, Integer)
    End Class

End Module
