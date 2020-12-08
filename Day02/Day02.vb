Module Day02
    Public Sub Day02_main()
        Dim testPWs As List(Of Day02_PW_Record) = Day02_ReadInput("Day02\Day02_test01.txt")
        Debug.Assert(testPWs.LongCount(Function(f) f.isValid) = 2)
        Debug.Assert(testPWs.LongCount(Function(f) f.isValid2) = 1)

        Dim PWs As List(Of Day02_PW_Record) = Day02_ReadInput("Day02\Day02_input.txt")
        Console.WriteLine("Day02 Part 1: " & PWs.LongCount(Function(f) f.isValid))
        Console.WriteLine("Day02 Part 2: " & PWs.LongCount(Function(f) f.isValid2))
    End Sub

    Public Class Day02_PW_Record
        Public MinValue As Integer
        Public MaxValue As Integer
        Public SearchChar As Char
        Public PW As String

        Public Function isValid() As Boolean
            Dim num = PW.LongCount(Function(f) f = SearchChar)
            Return ((num <= MaxValue) AndAlso (num >= MinValue))
        End Function

        Public Function isValid2() As Boolean
            Return (PW(MinValue - 1) = SearchChar) Xor (PW(MaxValue - 1) = SearchChar)
        End Function
    End Class

    Private Function Day02_ReadInput(inputpath As String) As List(Of Day02_PW_Record)
        Dim PWs As New List(Of Day02_PW_Record)
        Dim sr As New IO.StreamReader(inputpath)
        While (Not sr.EndOfStream)
            Dim line = sr.ReadLine 'eg: 1-3 a: abcde
            Dim newPW As New Day02_PW_Record
            newPW.MinValue = Convert.ToInt32(line.Split("-")(0))
            newPW.MaxValue = Convert.ToInt32(line.Split("-")(1).Split(" ")(0))
            newPW.SearchChar = line.Split(" ")(1).Split(":")(0).First
            newPW.PW = line.Split(":")(1).Trim
            PWs.Add(newPW)
        End While

        Return PWs
    End Function
End Module
