Module Day04
    Public Sub Day04_main()
        Dim testpath1 = "Day04\Day04_test01.txt"
        Dim testpath2 = "Day04\Day04_test02.txt"
        Dim testpath3 = "Day04\Day04_test03.txt"
        Dim inputpath = "Day04\Day04_input.txt"

        Dim testdata1 = Day04_ReadInput(testpath1)
        Dim inputdata = Day04_ReadInput(inputpath)
        Debug.Assert(Day04_Part1(testdata1) = 2, "Day04 Part1 failed")
        Console.WriteLine("Day04 Part1: " & Day04_Part1(inputdata))

        Dim testdata2 = Day04_ReadInput(testpath2)
        Dim testdata3 = Day04_ReadInput(testpath3)
        Debug.Assert(Day04_Part2(testdata2) = 0, "Day04 Part2 Case 1 failed")
        Debug.Assert(Day04_Part2(testdata3) = 4, "Day04 Part2 Case 2 failed")
        Console.WriteLine("Day04 Part2: " & Day04_Part2(inputdata))
    End Sub

    Private Function Day04_Part2(passportlist As List(Of Day04_Passport)) As Integer
        Return passportlist.LongCount(Function(f) f.isValid2)
    End Function

    Private Function Day04_Part1(passportlist As List(Of Day04_Passport)) As Integer
        Return passportlist.LongCount(Function(f) f.isValid)
    End Function

    Private Function Day04_ReadInput(path As String) As List(Of Day04_Passport)
        Dim result As New List(Of Day04_Passport)

        Dim sr As New IO.StreamReader(path)
        Dim passport As New Day04_Passport
        While (Not sr.EndOfStream)
            Dim line = sr.ReadLine()

            If line = "" Then
                result.Add(passport)
                passport = New Day04_Passport
                Continue While
            End If

            Dim elements = line.Split(" ")
            For Each element In elements
                Dim name = element.Split(":")(0)
                Dim value = element.Split(":")(1)
                Select Case name
                    Case "byr"
                        passport.byr = value
                    Case "iyr"
                        passport.iyr = value
                    Case "eyr"
                        passport.eyr = value
                    Case "hgt"
                        passport.hgt = value
                    Case "hcl"
                        passport.hcl = value
                    Case "ecl"
                        passport.ecl = value
                    Case "pid"
                        passport.pid = value
                    Case "cid"
                        passport.cid = value
                End Select
            Next

        End While
        result.Add(passport)

        Return result
    End Function

    Public Class Day04_Passport
        Public byr As String = "" '(Birth Year)
        Public iyr As String = "" '(Issue Year)
        Public eyr As String = "" '(Expiration Year)
        Public hgt As String = "" '(Height)
        Public hcl As String = "" '(Hair Color)
        Public ecl As String = "" '(Eye Color)
        Public pid As String = "" '(Passport ID)
        Public cid As String = "" '(Country ID)

        Public Function isValid() As Boolean
            Return (byr <> "" AndAlso iyr <> "" AndAlso eyr <> "" AndAlso hgt <> "" AndAlso hcl <> "" AndAlso ecl <> "" AndAlso pid <> "")
        End Function

        Public Function isValid2() As Boolean
            'valid
            If isValid() = False Then
                Return False
                Exit Function
            End If

            'byr
            Try
                Dim i = Convert.ToInt32(byr)
                If i < 1920 OrElse i > 2002 Then
                    Return False
                    Exit Function
                End If
            Catch ex As Exception
                Return False
                Exit Function
            End Try

            'iyr
            Try
                Dim i = Convert.ToInt32(iyr)
                If i < 2010 OrElse i > 2020 Then
                    Return False
                    Exit Function
                End If
            Catch ex As Exception
                Return False
                Exit Function
            End Try

            'eyr
            Try
                Dim i = Convert.ToInt32(eyr)
                If i < 2020 OrElse i > 2030 Then
                    Return False
                    Exit Function
                End If
            Catch ex As Exception
                Return False
                Exit Function
            End Try


            'hgt
            Try
                Dim unit As String
                unit = Right(hgt, 2)
                Select Case unit
                    Case "cm"
                        Dim i = Convert.ToInt32(hgt.Substring(0, hgt.Length - 2))
                        If i < 150 OrElse i > 193 Then
                            Return False
                            Exit Function
                        End If
                    Case "in"
                        Dim i = Convert.ToInt32(hgt.Substring(0, hgt.Length - 2))
                        If i < 59 OrElse i > 76 Then
                            Return False
                            Exit Function
                        End If
                    Case Else
                        Return False
                        Exit Function
                End Select
            Catch ex As Exception
                Return False
                Exit Function
            End Try

            'hcl
            Try
                Dim pattern = "^([#])+(([0-9]|[a-f]){6})$"
                If Text.RegularExpressions.Regex.IsMatch(hcl, pattern) = False Then
                    Return False
                    Exit Function
                End If
            Catch ex As Exception
                Return False
                Exit Function
            End Try


            'ecl
            Try
                Select Case ecl
                    Case "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
                        'nothing
                    Case Else
                        Return False
                        Exit Function
                End Select
            Catch ex As Exception
                Return False
                Exit Function
            End Try

            'pid
            Try
                Dim pattern = "^([0-9]{9})$"
                If Text.RegularExpressions.Regex.IsMatch(pid, pattern) = False Then
                    Return False
                    Exit Function
                End If
            Catch ex As Exception
                Return False
                Exit Function
            End Try

            Return True
        End Function
    End Class


End Module
