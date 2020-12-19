Imports System.Text.RegularExpressions

Module Day19
    Public Sub Day19_main()
        Dim testpath = "Day19\Day19_test01.txt"
        Dim testpath2 = "Day19\Day19_test02.txt"
        Dim inputpath = "Day19\Day19_input.txt"

        Dim testinput = Day19_ReadInput(testpath)
        Dim input = Day19_ReadInput(inputpath)

        'Console.WriteLine("0: " & input.CalculateRegex(0))
        Debug.Assert(Day19_Part01(testinput) = 2, "Day19 Part1 test1 failed")
        Console.WriteLine("Day19 Part1: " & Day19_Part01(input))

        Dim testinput2 = Day19_ReadInput(testpath2)
        Debug.Assert(Day19_Part02(testinput2) = 12, "Day19 Part2 test1 failed")
        Console.WriteLine("Day19 Part2: " & Day19_Part02(input))
    End Sub

    Public Function Day19_Part02(input As Day19_Input) As Long
        Debug.Assert(input.Rules(0) = "8 11")
        input.Rules(8) = "42 | 42 8" ' -> 42+
        input.Rules(11) = "42 31 | 42 11 31" '->42+ 31+ ?number of 42 and 31 should be equal -due to the rule8 it can be any number of 42 before, so I think it is not problem 

        'so rule(0): ((42)+)((42)+)((31)+) and second 42 and 31 number is the same
        'due to first 42+ it means that number of 42+ must be more than matches of 31 at the end
        'Dim regex0 = "((" & input.CalculateRegex(42) & ")+)" '8
        Dim pattern42 = input.CalculateRegex(42)
        Dim pattern31 = input.CalculateRegex(31)

        'Console.WriteLine("42: " & pattern42)
        'Console.WriteLine("31: " & pattern31)
        Dim regex0 = "^((" & pattern42 & ")+)((" & pattern31 & ")+)$"
        Dim num As Integer = 0
        For Each mes In input.Messages
            If Regex.IsMatch(mes, regex0) Then
                'further check needed to check if num of 42 is more than num of 31
                Dim substring = mes
                While True
                    Dim num42 = Regex.Matches(substring, "^" & pattern42)
                    Dim num31 = Regex.Matches(substring, pattern31 & "$")

                    If num42.Count > 0 Then
                        If num31.Count > 0 Then
                            substring = substring.Remove(0, num42.First.Length)
                            substring = substring.Remove(substring.Length - num31.Last.Length, num31.Last.Length)
                        Else
                            num += 1
                            Exit While
                        End If
                    Else
                        'no extra 42
                        Exit While
                    End If

                End While
            End If
        Next
        Return num
    End Function

    Public Function Day19_Part01(input As Day19_Input) As Long
        Return input.Messages.LongCount(Function(f) Regex.IsMatch(f, input.Regex))
    End Function


    Public Function Day19_ReadInput(path As String) As Day19_Input
        Dim result As New Day19_Input
        Dim sr As New IO.StreamReader(path)

        result.Rules = New Dictionary(Of Integer, String)
        While Not sr.EndOfStream
            '0: 4 1 5
            Dim line = sr.ReadLine
            If line = "" Then Exit While
            Dim key = Convert.ToInt32(line.Split(":")(0))
            Dim value = line.Split(":")(1).Trim
            result.Rules.Add(key, value)
        End While

        result.Messages = New List(Of String)
        While Not sr.EndOfStream
            Dim line = sr.ReadLine
            'ababbb
            result.Messages.Add(line)
        End While

        Return result
    End Function

    Public Class Day19_Input
        Public Rules As Dictionary(Of Integer, String)
        Public Messages As List(Of String)
        Private _Regex As String = ""
        Public ReadOnly Property Regex As String
            Get
                If _Regex = "" Then '"^([0-9]{9})$"
                    _Regex = "^" & CalculateRegex(0) & "$"
                End If

                Return _Regex
            End Get
        End Property

        Public Function CalculateRegex(key As Integer) As String
            Dim line = Rules(key).Trim
            If line.Contains("""") Then 'a or b line
                Return "(" + line.Replace("""", "") + ")"
            Else
                Dim elements = line.Split(" ")
                Dim result As String = "(("
                For Each element In elements
                    If element = "|" Then
                        result += ")|("
                    Else
                        result += CalculateRegex(Convert.ToInt32(element))
                    End If
                Next
                result += "))"
                Return result
            End If
        End Function
    End Class
End Module
