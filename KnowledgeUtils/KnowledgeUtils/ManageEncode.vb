
Namespace Encode

    Public Class ManageEncode

        ''' <summary>
        ''' Sault คือสิ่งที่จะประสมกับ Password ที่ถูก Encode แล้วจะนำสองสิ่งนี้ผสมกันแล้วนำมา Decode ให้เป็น password จริง
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSalt() As String
            Dim Rand As New Random
            Dim Result As String = ""
            For index As Integer = 1 To 3
                Result &= ChrW(Rand.Next(32, 126))
            Next
            Return Result
        End Function

        ''' <summary>
        ''' ฟังชั่นทำการเข้าระหัส Password กับ Salt ผสมกัน 
        ''' </summary>
        ''' <param name="Word"></param>
        ''' <param name="Salt"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Encode(ByVal Word As String, ByVal Salt As String) As String
            '<<< หาตัวกลางของ Word ก่อนถ้าเป็นเลขคี่+1
            Dim NumWord = Word.Length
            Dim Ntemp = Math.Round(NumWord / 2, 0, MidpointRounding.AwayFromZero)
            Dim MidWord = If(Ntemp Mod 2 = 1, Ntemp - 1, Ntemp)
            'เก็บ salt ใส่กล่อง
            Dim BoxSalt(2) As Integer
            For index As Integer = 0 To 2
                BoxSalt(index) = AscW(Strings.Mid(Salt, index + 1, 1))
            Next

            '<<< เอา Salt สามตัวมาใส่ หน้า กลาง หลัง
            Dim BoxPass(NumWord + 2) As Integer
            If MidWord > 0 Then
                'กรณีมี password มากกว่า 2 ตัว
                Dim IndexWord As Integer = 1
                For index As Integer = 0 To BoxPass.Length - 1
                    Select Case index
                        Case 0
                            BoxPass(index) = BoxSalt(0)
                        Case MidWord
                            BoxPass(index) = BoxSalt(1)
                        Case BoxPass.Length - 1
                            BoxPass(index) = BoxSalt(2)
                        Case Else
                            BoxPass(index) = AscW(Strings.Mid(Word, IndexWord, 1))
                            IndexWord += 1
                    End Select
                Next
            Else
                'กรณีมี password มากกว่า 1,2 ตัว
                Dim IndexWord As Integer = 1
                For index As Integer = 0 To BoxPass.Length - 1
                    Select Case index
                        Case 0
                            BoxPass(index) = BoxSalt(0)
                        Case 1
                            BoxPass(index) = BoxSalt(1)
                        Case 2
                            BoxPass(index) = BoxSalt(2)
                        Case Else
                            BoxPass(index) = AscW(Strings.Mid(Word, IndexWord, 1))
                            IndexWord += 1
                    End Select
                Next
            End If

            '<<< ประกอบตัวอักษร
            Dim PassEndCode As String = ""
            Dim AscWord As Integer = 0
            For index As Integer = 0 To BoxPass.Length - 1
                AscWord += BoxPass(index)
                If AscWord > 126 Then
                    AscWord = AscWord - 126
                End If
                If AscWord < 32 Then
                    AscWord = AscWord + 32
                End If
                PassEndCode &= ChrW(AscWord)
            Next
            Return PassEndCode
        End Function

    End Class

End Namespace


