Imports System.IO
Imports System.Windows.Forms

Namespace IO

    Public Class ManageFile
        Private StrPath As String = ""
        Private StrFileName As String = ""
        Private StrFullFileName As String = ""
        Private StrFilePathName As String = ""
        Private Sr As StreamReader

#Region "Property"

        ''' <summary>
        ''' คืนค่า Path
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PathName() As String
            Get
                If StrFilePathName <> "" Then
                    Dim Fi As New FileInfo(StrFilePathName)
                    StrPath = Fi.DirectoryName
                End If
                Return StrPath
            End Get
        End Property

        ''' <summary>
        ''' คืนค่า File + Path
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PathFullName() As String
            Get
                If StrFilePathName <> "" Then
                    Dim Fi As New FileInfo(StrFilePathName)
                    StrFilePathName = Fi.FullName
                End If
                Return StrFilePathName
            End Get
        End Property

        ''' <summary>
        ''' คืนค่าชื่อไฟล์ + นามสกุลไฟล์
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FullFileName() As String
            Get
                If StrFilePathName <> "" Then
                    Dim Fi As New FileInfo(StrFilePathName)
                    StrFullFileName = Fi.Name
                End If
                Return StrFullFileName
            End Get
        End Property

        ''' <summary>
        ''' คืนค่าขนาดข้อมูล
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FileSize() As Long
            Get
                If StrFilePathName <> "" Then
                    Dim Fi As New FileInfo(StrFilePathName)
                    Return Fi.Length
                End If
                Return 0
            End Get
        End Property

        ''' <summary>
        ''' คืนค่าชื่อของไฟล์
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FileName() As String
            Get
                If StrFilePathName <> "" Then
                    Dim Fi As New FileInfo(StrFilePathName)
                    StrFullFileName = Fi.Name
                    Dim ArName As String() = Split(StrFullFileName, ".")
                    StrFileName = ArName(0)
                End If
                Return StrFileName
            End Get
        End Property

        ''' <summary>
        ''' คืนค่านามสกุลของไฟล์ มีจุดอยุ่ข้างหน้าด้วยนะ ตัวอย่าง .jpg
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property FileExtensionName() As String
            Get
                Dim ExtensionName = ""
                If StrFilePathName <> "" Then
                    Dim Fi As New FileInfo(StrFilePathName)
                    ExtensionName = Fi.Extension
                End If
                Return ExtensionName
            End Get
        End Property

#End Region

        Sub New()
        End Sub

        Sub New(ByVal FullPath As String)
            StrFilePathName = FullPath
        End Sub

        ''' <summary>
        ''' เช็คไฟล์ที่ต้องการค้นหา
        ''' </summary>
        ''' <param name="FileName">Path + filename</param>
        ''' <returns>yes=พบไฟล์,false=ไม่พบ</returns>
        ''' <remarks></remarks>
        Public Function CheckFile(Optional ByVal FileName As String = "") As Boolean
            If FileName = "" Then
                FileName = PathFullName
            End If
            If File.Exists(FileName) Then
                CheckFile = True
            Else
                CheckFile = False
            End If
        End Function

        ''' <summary>
        ''' เช็คแฟ้มที่ต้องการค้นหา
        ''' </summary>
        ''' <param name="FolderName">ชื่อแฟ้มที่ต้องการค้นหา</param>
        ''' <param name="Path">Path ที่จะเช็คแฟ้มที่ต้องการค้นหาถ้าไม่ระบุจะหมายถึง path app เหมาะกับ app ถ้าเป็น web ควรระบุ</param>
        ''' <returns>yes=พบแฟ้ม,false=ไม่พบแฟ้ม</returns>
        ''' <remarks>กรณีไม่ระบุ Path จะใช้ Path ของ Project</remarks>
        Public Function CheckFolder(Optional ByVal FolderName As String = "", Optional ByVal Path As String = "") As Boolean
            Dim Folder As String
            If Path = "" Then
                Path = Application.StartupPath
            End If
            Folder = Path & "\" & FolderName & "\"
            If FolderName = "" Then
                Folder = PathName
            End If
            If Directory.Exists(Folder) Then
                CheckFolder = True
            Else
                CheckFolder = False
            End If
        End Function

        ''' <summary>
        ''' ลบไฟล์ที่อยู่ในแฟ้มทั้งหมด
        ''' </summary>
        ''' <param name="FolderName">ชื่อแฟ้มที่ต้องการจะลบไฟล์ที่อยู่ข้างใน</param>
        ''' <param name="Path">Path ของแฟ้มที่ต้องการจะลบไฟล์ ถ้าไม่ระบุจะหมายถึง path app เหมาะกับ app ถ้าเป็น web ควรระบุ</param>
        ''' <param name="FilePatern">รูปแบบไฟล์ที่จะลบ Default = *.*</param>
        ''' <param name="RecycleBin">กรณีที่ไม่ได้ระบุค่าจะหมายถึง ไม่เก็บไฟล์ไว้ที่ RecycleBin</param>
        ''' <remarks>กรณีไม่ระบุ Path จะใช้ Path ของ Project</remarks>
        Public Sub DeleteFileInFolder(Optional ByVal FolderName As String = "", Optional ByVal Path As String = "", Optional ByVal FilePatern As String = "*.*", _
        Optional ByVal RecycleBin As Microsoft.VisualBasic.FileIO.RecycleOption = FileIO.RecycleOption.DeletePermanently)
            Dim Folder As String
            If Path = "" Then
                Path = Application.StartupPath
            End If
            Folder = Path & "\" & FolderName
            If FolderName = "" Then
                Folder = PathName
            End If
            For Each foundFile As String In My.Computer.FileSystem.GetFiles(Folder, _
               FileIO.SearchOption.SearchAllSubDirectories, "*.*")
                DeleteFile(foundFile, RecycleBin)
            Next
        End Sub

        ''' <summary>
        ''' สร้างแฟ้ม
        ''' </summary>
        ''' <param name="FolderName">ชื่อแฟ้มที่ต้องการสร้าง</param>
        ''' <param name="Path">Path ที่จะสร้างแฟ้ม ถ้าไม่ระบุจะหมายถึง path app เหมาะกับ app ถ้าเป็น web ควรระบุ</param>
        ''' <remarks>กรณีไม่ระบุ Path ใจะใช้ Path ของ Project</remarks>
        Public Sub CreateFolder(Optional ByVal FolderName As String = "", Optional ByVal Path As String = "")
            Dim Folder As String
            If Path = "" Then
                Path = Application.StartupPath
            End If
            Folder = Path & "\" & FolderName
            If FolderName = "" Then
                Folder = PathName
            End If
            My.Computer.FileSystem.CreateDirectory(Folder)
        End Sub

        ''' <summary>
        ''' สร้างไฟล์
        ''' </summary>
        ''' <param name="FileName">Path + filename</param>
        ''' <param name="TextInput">ข้อความที่จะใส่เข้าไปในไฟล์</param>
        ''' <param name="Append">true=เขียนข้อความต่อท้ายของเก่า,false=เขียนทับ</param>
        ''' <remarks></remarks>
        Public Sub CreateFile(Optional ByVal FileName As String = "", Optional ByVal TextInput As String = "", Optional ByVal Append As Boolean = False)
            If FileName = "" Then
                FileName = PathFullName
            End If
            TextInput = TextInput & Environment.NewLine
            My.Computer.FileSystem.WriteAllText(FileName, TextInput, Append, Text.Encoding.Default)
        End Sub

        ''' <summary>
        ''' ลบไฟล์
        ''' </summary>
        ''' <param name="FileName">Path + filename</param>
        ''' <param name="RecycleBin">กรณีที่ไม่ได้ระบุค่าจะหมายถึง ไม่เก็บไฟล์ไว้ที่ RecycleBin</param>
        ''' <remarks></remarks>
        Public Sub DeleteFile(Optional ByVal FileName As String = "", Optional ByVal RecycleBin As Microsoft.VisualBasic.FileIO.RecycleOption = FileIO.RecycleOption.DeletePermanently)
            If FileName = "" Then
                FileName = PathFullName
            End If
            If CheckFile(FileName) = True Then
                My.Computer.FileSystem.DeleteFile(FileName, FileIO.UIOption.OnlyErrorDialogs, RecycleBin)
            End If
        End Sub

        ''' <summary>
        ''' คัดลอกไฟล์
        ''' </summary>
        ''' <param name="FileName">Path + filename</param>
        ''' <param name="FilePathDestination">สถานที่ปลายทางที่จะคัดลอกลงไป+ชื่อไฟล์</param>
        ''' <param name="OverWrite">กรณีไม่ระบุจะหมายถึงคัดลอกทับ</param>
        ''' <remarks></remarks>
        Public Sub CopyFile(ByVal FilePathDestination As String, Optional ByVal FileName As String = "", Optional ByVal OverWrite As Boolean = True)
            If FileName = "" Then
                FileName = PathFullName
            End If
            If CheckFile(FileName) = True Then
                My.Computer.FileSystem.CopyFile(FileName, FilePathDestination, OverWrite)
            End If
        End Sub

        ''' <summary>
        ''' อ่านไฟล์ทั้งหมด
        ''' </summary>
        ''' <param name="FileName">Path + filename</param>
        ''' <returns>ข้อความในไฟล์</returns>
        ''' <remarks></remarks>
        Public Function Readfile(Optional ByVal FileName As String = "") As String
            If FileName = "" Then
                FileName = PathFullName
            End If
            Readfile = My.Computer.FileSystem.ReadAllText(FileName, Text.Encoding.Default)
        End Function

        ''' <summary>
        ''' เปิดไฟล์ที่จะอ่าน
        ''' </summary>
        ''' <param name="FileName">Path + filename</param>
        ''' <remarks>เปิดเพื่อจะอ่านข้อมูลในไฟล์</remarks>
        Public Sub OpenFile(Optional ByVal FileName As String = "")
            If FileName = "" Then
                FileName = PathFullName
            End If
            Sr = New StreamReader(FileName, Text.Encoding.Default)
        End Sub

        ''' <summary>
        ''' อ่านไฟล์ทีละบรรทัด
        ''' </summary>
        ''' <returns>คืนค่าที่อ่านมาทีละบรรทัด</returns>
        ''' <remarks></remarks>
        Public Function ReadLine() As String
            ReadLine = Sr.ReadLine
        End Function

        ''' <summary>
        ''' อ่านไฟล์ทีละตัวอักษร(กรณีที่จบไฟล์คืนค่า=EndOfFile,กรณีที่จบบรรทัด=LineFeed,กรณีที่สุดบรรทัด=Return)
        ''' </summary>
        ''' <param name="ForChr">1=คืนค่า CHR,0=คืนค่า ASC</param>
        ''' <returns>คืนค่าที่อ่านมาทีละตัวอักษร(กรณีที่จบไฟล์คืนค่า=EndOfFile,กรณีที่จบบรรทัด=LineFeed,กรณีที่สุดบรรทัด=Return)</returns>
        ''' <remarks></remarks>
        Public Function Read(Optional ByVal ForChr As SByte = 1) As String
            Dim ForAsc As Integer
            ForAsc = Sr.Read
            If ForAsc = -1 Then
                Read = "EndOfFile"
                Exit Function
            End If
            If ForChr = 1 Then
                If ForAsc = 10 Then
                    Read = "LineFeed"
                ElseIf ForAsc = 13 Then
                    Read = "Return"
                Else
                    Read = ChrW(ForAsc)
                End If
            Else
                Read = ForAsc
            End If
        End Function

        ''' <summary>
        ''' ปิดไฟล์ที่อ่านอยู่
        ''' </summary>
        ''' <remarks>ควรใช้เมื่อมีการเปิดไฟล์ทิ้งไว้เฉพาะกลุ่มคำสั่งอ่านไฟล์ควรปิดด้วย</remarks>
        Public Sub CloseFile()
            If Sr Is Nothing Then
                Exit Sub
            End If
            Sr.Close()
            Sr.Dispose()
        End Sub

        ''' <summary>
        ''' เช็คตัวอักษรที่ส่งมาเพื่อตรวจสอบว่าเป็นตัวอักษรประเภทไหน
        ''' </summary>
        ''' <param name="Charecter">คืนค่าเป็นภาษา</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckLanguage(ByVal Charecter As String) As Language
            If (Asc(Charecter) >= 97 AndAlso Asc(Charecter) <= 122) OrElse (Asc(Charecter) >= 65 AndAlso Asc(Charecter) <= 90) Then
                CheckLanguage = Language.Eng
            ElseIf (Asc(Charecter) >= 161 AndAlso Asc(Charecter) <= 251) Then
                CheckLanguage = Language.Tha
            ElseIf (Asc(Charecter) >= 48 AndAlso Asc(Charecter) <= 57) Then
                CheckLanguage = Language.Math
            Else
                CheckLanguage = Language.Other
            End If
        End Function

        Public Enum Language
            Tha = 0
            Eng = 1
            Math = 2
            Other = 3
        End Enum

    End Class

End Namespace


