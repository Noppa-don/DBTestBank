
Imports System.IO
Imports System.Text

#If DEBUG Then
Imports NUnit.Framework
#End If

''' <summary>
''' A StreamReader that excludes XML-illegal characters while reading.
''' </summary>
Public Class XmlSanitizingStream
	Inherits StreamReader
	''' <summary>
	''' The charactet that denotes the end of a file has been reached.
	''' </summary>
	Private Const EOF As Integer = -1

	''' <summary>Create an instance of XmlSanitizingStream.</summary>
	''' <param name="streamToSanitize">
	''' The stream to sanitize of illegal XML characters.
	''' </param>
	Public Sub New(streamToSanitize As Stream)
		MyBase.New(streamToSanitize, True)
	End Sub

	''' <summary>
	''' Get whether an integer represents a legal XML 1.0 or 1.1 character. See
	''' the specification at w3.org for these characters.
	''' </summary>
	''' <param name="xmlVersion">
	''' The version number as a string. Use "1.0" for XML 1.0 character
	''' validation, and use "1.1" for XML 1.1 character validation.
	''' </param>
	Public Shared Function IsLegalXmlChar(xmlVersion As String, character As Integer) As Boolean
		Select Case xmlVersion
			Case "1.1"
				' http://www.w3.org/TR/xml11/#charsets
				If True Then
					Return Not (character <= &H8 OrElse character = &Hb OrElse character = &Hc OrElse (character >= &He AndAlso character <= &H1f) OrElse (character >= &H7f AndAlso character <= &H84) OrElse (character >= &H86 AndAlso character <= &H9f) OrElse character > &H10ffff)
				End If
			Case "1.0"
				' http://www.w3.org/TR/REC-xml/#charsets
				If True Then
					' == '\t' == 9   
					' == '\n' == 10  
					' == '\r' == 13  
					Return (character = &H9 OrElse character = &Ha OrElse character = &Hd OrElse (character >= &H20 AndAlso character <= &Hd7ff) OrElse (character >= &He000 AndAlso character <= &Hfffd) OrElse (character >= &H10000 AndAlso character <= &H10ffff))
				End If
			Case Else
				If True Then
					Throw New ArgumentOutOfRangeException("xmlVersion", String.Format("'{0}' is not a valid XML version."))
				End If
		End Select
	End Function

	''' <summary>
	''' Get whether an integer represents a legal XML 1.0 character. See the  
	''' specification at w3.org for these characters.
	''' </summary>
	Public Shared Function IsLegalXmlChar(character As Integer) As Boolean
		Return XmlSanitizingStream.IsLegalXmlChar("1.0", character)
	End Function

	Public Overrides Function Read() As Integer
		' Read each character, skipping over characters that XML has prohibited

		Dim nextCharacter As Integer

		Do
			' Read a character

			If (InlineAssignHelper(nextCharacter, MyBase.Read())) = EOF Then
				' If the character denotes the end of the file, stop reading

				Exit Do
			End If

		' Skip the character if it's prohibited, and try the next

		Loop While Not XmlSanitizingStream.IsLegalXmlChar(nextCharacter)

		Return nextCharacter
	End Function

	Public Overrides Function Peek() As Integer
		' Return the next legl XML character without reading it 

		Dim nextCharacter As Integer

		Do
			' See what the next character is 

			nextCharacter = MyBase.Peek()
		' If it's prohibited XML, skip over the character in the stream
		' and try the next.

		Loop While Not XmlSanitizingStream.IsLegalXmlChar(nextCharacter) AndAlso (InlineAssignHelper(nextCharacter, MyBase.Read())) <> EOF

		Return nextCharacter

	End Function
	' method
	#Region "Read*() method overrides"

	' The following methods are exact copies of the methods in TextReader, 
	' extracting by disassembling it in Refelctor

	Public Overrides Function Read(buffer As Char(), index As Integer, count As Integer) As Integer
		If buffer Is Nothing Then
			Throw New ArgumentNullException("buffer")
		End If
		If index < 0 Then
			Throw New ArgumentOutOfRangeException("index")
		End If
		If count < 0 Then
			Throw New ArgumentOutOfRangeException("count")
		End If
		If (buffer.Length - index) < count Then
			Throw New ArgumentException()
		End If
		Dim num As Integer = 0
		Do
			Dim num2 As Integer = Me.Read()
			If num2 = -1 Then
				Return num
			End If
            buffer(index + System.Math.Max(System.Threading.Interlocked.Increment(num), num - 1)) = CChar(ChrW(num2))
		Loop While num < count
		Return num
	End Function

	Public Overrides Function ReadBlock(buffer As Char(), index As Integer, count As Integer) As Integer
		Dim num As Integer
		Dim num2 As Integer = 0
		Do
			num2 += InlineAssignHelper(num, Me.Read(buffer, index + num2, count - num2))
		Loop While (num > 0) AndAlso (num2 < count)
		Return num2
	End Function

	Public Overrides Function ReadLine() As String
		Dim builder As New StringBuilder()
		While True
			Dim num As Integer = Me.Read()
			Select Case num
				Case -1
					If builder.Length > 0 Then
						Return builder.ToString()
					End If
					Return Nothing

				Case 13, 10
					If (num = 13) AndAlso (Me.Peek() = 10) Then
						Me.Read()
					End If
					Return builder.ToString()
			End Select
            builder.Append(CChar(ChrW(num)))
		End While
	End Function

	Public Overrides Function ReadToEnd() As String
		Dim num As Integer
		Dim buffer As Char() = New Char(4095) {}
		Dim builder As New StringBuilder(&H1000)
		While (InlineAssignHelper(num, Me.Read(buffer, 0, buffer.Length))) <> 0
			builder.Append(buffer, 0, num)
		End While
		Return builder.ToString()
	End Function
	Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, value As T) As T
		target = value
		Return value
	End Function

	#End Region

End Class
' class
#If DEBUG Then

<TestFixture> _
Public Class XmlSanitizingStreamTest

	<Test> _
	Public Sub ReadOnlyReturnsLegalXmlCharacters()
		' This should be stripped to "\t\r\n<>:"

		Dim xml As String = vbNullChar & vbTab & ChrW(7) & vbCr & vbBack & vbLf & "<>:"

		' Load the XML as a Stream

		Using buffer = New MemoryStream(System.Text.Encoding.[Default].GetBytes(xml))
			Using sanitizer = New XmlSanitizingStream(buffer)
				Assert.AreEqual(sanitizer.Read(), ControlChars.Tab)
				Assert.AreEqual(sanitizer.Read(), ControlChars.Cr)
				Assert.AreEqual(sanitizer.Read(), ControlChars.Lf)
				Assert.AreEqual(sanitizer.Read(), "<"C)
				Assert.AreEqual(sanitizer.Read(), ">"C)
				Assert.AreEqual(sanitizer.Read(), ":"C)
				Assert.AreEqual(sanitizer.Read(), -1)
				Assert.IsTrue(sanitizer.EndOfStream)
			End Using
		End Using

		Using buffer = New MemoryStream(System.Text.Encoding.[Default].GetBytes(xml))
			Using sanitizer = New XmlSanitizingStream(buffer)
				Assert.AreEqual(sanitizer.ReadToEnd(), vbTab & vbCr & vbLf & "<>:")
			End Using
		End Using

	End Sub
	' method
End Class
' class
#End If