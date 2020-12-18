Module Day16
    Public Sub Day16_main()
        Dim testpath = "Day16\Day16_test01.txt"
        Dim testpath2 = "Day16\Day16_test02.txt"
        Dim inputpath = "Day16\Day16_input.txt"

        Dim testinput = Day16_ReadInput(testpath)
        Dim testinput2 = Day16_ReadInput(testpath2)
        Dim input = Day16_ReadInput(inputpath)

        Debug.Assert(Day16_Part01(testinput) = 71, "Day16 Part1 test1 failed")
        Console.WriteLine("Day16 Part1: " & Day16_Part01(input))

        Debug.Assert(Day16_Part02(testinput2) = 1, "Day16 Part2 test1 failed") 'need to test manually
        Console.WriteLine("Day16 Part2: " & Day16_Part02(input))
    End Sub

    Public Function Day16_Part01(input As Day16_input) As Integer
        Return input.OtherTickets.Sum(Function(f) f.Sum(Function(g) If(input.Validators.Any(Function(h) h.isNumberValid(g)), 0, g)))
    End Function

    Public Function Day16_Part02(input As Day16_input) As Int64
        'remove invalid
        'input.OtherTickets.RemoveAll(Function(f) f.Sum(Function(g) If(input.Validators.Any(Function(h) h.isNumberValid(g)), 0, g)) > 0)
        Dim otherfilter As New List(Of List(Of Integer))
        Dim colums = input.OtherTickets(0).Count
        For Each ticket In input.OtherTickets
            If ticket.LongCount(Function(g) input.Validators.Any(Function(h) h.isNumberValid(g))) = colums Then otherfilter.Add(ticket)
        Next
        input.OtherTickets = otherfilter

        'add my ticket to otherticket list - not sure if necessary, it helps or makes it more hard to solve
        input.OtherTickets.Add(input.MyTicket)

        Dim PossibleNames As New List(Of List(Of String))
        'iterate through positions
        For i = 0 To input.OtherTickets(0).Count - 1
            Dim index = i
            PossibleNames.Add(input.Validators.FindAll(Function(f) input.OtherTickets.All(Function(g) f.isNumberValid(g(index)))).ConvertAll(Function(h) h.FieldName))
        Next

        Dim changed As Boolean
        Do
            changed = False
            'collecting names where only one name is possible
            Dim namefound As New List(Of String)
            For i = 0 To PossibleNames.Count - 1
                If PossibleNames(i).Count = 1 Then
                    namefound.Add(PossibleNames(i)(0))
                End If
            Next

            For i = 0 To PossibleNames.Count - 1
                If PossibleNames(i).Count > 1 Then
                    For Each name In namefound
                        If PossibleNames(i).Contains(name) Then
                            changed = True
                            PossibleNames(i).Remove(name)
                        End If
                    Next
                End If
            Next

            'collecting names that possible in only one place
            'For Each valid In input.Validators
            '    If namefound.Contains(valid.FieldName) Then Continue For

            '    If PossibleNames.LongCount(Function(f) f.Contains(valid.FieldName)) = 1 Then 'that possible name could only contain this field name
            '        Dim index2 = PossibleNames.FindIndex(Function(f) f.Contains(valid.FieldName))
            '        PossibleNames(index2) = {valid.FieldName}.ToList
            '    End If
            'Next
        Loop While changed

        'handle input that can contain possible names = 0
        'Dim notgivenName As String = ""
        'For Each valid In input.Validators
        '    If PossibleNames.LongCount(Function(f) f.Contains(valid.FieldName)) = 0 Then
        '        notgivenName = valid.FieldName
        '    End If
        'Next
        'If notgivenName <> "" Then
        '    Dim index3 = PossibleNames.FindIndex(Function(f) f.Count = 0)
        '    PossibleNames(index3) = {notgivenName}.ToList
        'End If


        Dim multi As Int64 = 1
        For i = 0 To input.MyTicket.Count - 1
            If PossibleNames(i).Count > 0 Then
                ' Console.WriteLine(PossibleNames(i)(0) & ": " & input.MyTicket(i))
                If PossibleNames(i)(0).Contains("departure") Then multi *= Convert.ToInt64(input.MyTicket(i))
            End If

        Next
        '14955328459 - too low
        Return multi
    End Function

    Public Function Day16_ReadInput(path As String) As Day16_input
        Dim result As New Day16_input

        Dim sr As New IO.StreamReader(path)
        Dim line As String = ""
        'read validators
        result.Validators = New List(Of Day16_Validator)
        While Not sr.EndOfStream
            'eg: class: 1-3 or 5-7
            line = sr.ReadLine
            If line = "" Then Exit While
            Dim fieldname = line.Split(":")(0).Trim
            Dim low1 = Convert.ToInt32(line.Split(":")(1).Split("-")(0).Trim)
            Dim high1 = Convert.ToInt32(line.Split("-")(1).Split("or")(0).Trim)
            Dim low2 = Convert.ToInt32(line.Split("-")(1).Split("or")(1).Trim)
            Dim high2 = Convert.ToInt32(line.Split("-")(2).Trim)
            result.Validators.Add(New Day16_Validator(fieldname, low1, high1, low2, high2))
        End While

        'read tickets
        line = sr.ReadLine '"your ticket"
        line = sr.ReadLine
        result.MyTicket = line.Split(",").ToList.ConvertAll(Function(f) Convert.ToInt32(f))

        line = sr.ReadLine 'empty line
        line = sr.ReadLine '"other ticket"

        'read other tickets
        result.OtherTickets = New List(Of List(Of Integer))
        While Not sr.EndOfStream
            'eg: class: 1-3 or 5-7
            line = sr.ReadLine
            result.OtherTickets.Add(line.Split(",").ToList.ConvertAll(Function(f) Convert.ToInt32(f)))
        End While

        Return result
    End Function

    Public Class Day16_input
        Public Validators As List(Of Day16_Validator)
        Public MyTicket As List(Of Integer)
        Public OtherTickets As List(Of List(Of Integer))
    End Class

    Public Class Day16_Validator
        Public FieldName As String
        Public Low1 As Integer
        Public High1 As Integer
        Public Low2 As Integer
        Public High2 As Integer

        Public Sub New(fieldname As String, low1 As Integer, high1 As Integer, low2 As Integer, high2 As Integer)
            Me.FieldName = fieldname
            Me.Low1 = low1
            Me.High1 = high1
            Me.Low2 = low2
            Me.High2 = high2
        End Sub

        Public Function isNumberValid(input As Integer) As Boolean
            Return ((input >= Low1) AndAlso (input <= High1)) OrElse ((input >= Low2) AndAlso (input <= High2))
        End Function
    End Class
End Module
