Module Day21
    Public Sub Day21_main()
        Dim testpath = "Day21\Day21_test01.txt"
        Dim inputpath = "Day21\Day21_input.txt"

        Dim testinput = Day21_ReadInput(testpath)
        Dim input = Day21_ReadInput(inputpath)

        Debug.Assert(Day21(testinput).Item1 = 5, "Day21 test2 failed")
        Debug.Assert(Day21(testinput).Item2 = "mxmxvkd,sqjhc,fvjkl", "Day21 test2 failed")

        Dim result = Day21(input)
        Console.WriteLine("Day21 Part1: " & Day21(input).Item1)
        Console.WriteLine("Day21 Part1: " & Day21(input).Item2)
    End Sub


    Public Function Day21(input As List(Of Day21_Food)) As (Integer, String)
        Dim allergens As New SortedDictionary(Of String, String)
        Dim ingredient As New Dictionary(Of String, String)
        For Each food In input
            For Each aller In food.Allergens
                If allergens.ContainsKey(aller) = False Then allergens.Add(aller, "")
            Next
            For Each ing In food.Ingredients
                If ingredient.ContainsKey(ing) = False Then ingredient.Add(ing, "")
            Next
        Next

        Dim possibleAllergens As New Dictionary(Of String, List(Of String))
        For Each aller In allergens.Keys
            Dim ings As New List(Of String)
            For Each food In input
                If food.Allergens.Contains(aller) Then
                    If ings.Count = 0 Then
                        ings.AddRange(food.Ingredients)
                    Else
                        ings = ings.Intersect(food.Ingredients).ToList
                    End If
                End If
            Next
            possibleAllergens.Add(aller, ings)
        Next

        Dim changed = True
        While changed
            changed = False
            For Each posaller In possibleAllergens
                If posaller.Value.Count = 1 Then
                    allergens(posaller.Key) = posaller.Value.First
                    ingredient(posaller.Value.First) = posaller.Key
                End If
            Next
            For Each ing In ingredient
                If ing.Value <> "" Then
                    For Each posaller In possibleAllergens
                        If posaller.Key = ing.Value Then Continue For
                        If posaller.Value.Contains(ing.Key) Then
                            posaller.Value.Remove(ing.Key)
                            changed = True
                        End If
                    Next
                End If
            Next
        End While

        Dim result1 = 0
        For Each food In input
            For Each ing In food.Ingredients
                If ingredient(ing) = "" Then result1 += 1
            Next
        Next

        Dim result2 = String.Join(",", allergens.ToList.ConvertAll(Function(f) f.Value))
        Return (result1, result2)
    End Function

    Public Function Day21_ReadInput(path As String) As List(Of Day21_Food)
        Dim result As New List(Of Day21_Food)

        Dim sr As New IO.StreamReader(path)
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            Dim food1 As New Day21_Food
            food1.Ingredients = line.Split("(")(0).Trim.Split(" ").ToList
            food1.Allergens = line.Split("(")(1).Trim.TrimEnd(")").Split(",").ToList.ConvertAll(Function(f) f.Split("contains").Last.Trim)
            result.Add(food1)
        End While

        Return result
    End Function

    Public Class Day21_Food
        Public Ingredients As New List(Of String)
        Public Allergens As New List(Of String)
    End Class
End Module
